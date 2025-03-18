using Microsoft.OData.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.OData.Client
{
    public interface IODataQueryBuilder<T>
    {
        Task<IEnumerable<T>> QueryAsync(Expression<Func<IQueryable<T>, IQueryable<T>>> queryExp = null);
        Task<IEnumerable<T>> QueryAsync(Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> queryExp);
        Task<IEnumerable<R>> QueryAsync<R>(Expression<Func<IQueryable<T>, IQueryable<R>>> queryExp);
        Task<T> QueryAsync(Expression<Func<IQueryable<T>, T>> queryExp);
        Task<R> QueryAsync<R>(Expression<Func<IQueryable<T>, R>> queryExp);
    }

    public class ODataQueryBuilder<T> : IODataQueryBuilder<T>
    {
        delegate Task<T[]> FetchODataDelegate(string queryString);
        delegate Task<long> FetchCountODataDelegate(string queryString);


        const string ServiceUri = "http://fakeodataendpoint";

        readonly FetchODataDelegate _fetchODataDelegate;
        readonly FetchCountODataDelegate _fetchCountODataDelegate;
        private ODataQueryBuilder(FetchODataDelegate fetchODataDelegate, FetchCountODataDelegate fetchCountODataDelegate)
        {
            _fetchODataDelegate = fetchODataDelegate;
            _fetchCountODataDelegate = fetchCountODataDelegate;
        }


        IQueryable GetDataServiceQuery(LambdaExpression queryExp)
        {
            var ctx = new DataServiceContext(new Uri(ServiceUri));
            var odataQuery = ctx.CreateQuery<T>(typeof(T).Name);
            odataQuery = odataQuery.IncludeCount();

            return queryExp != null ? queryExp.Compile().DynamicInvoke(odataQuery) as IQueryable : odataQuery;
        }

        public static IODataQueryBuilder<T> Create<Tp>(IODataFetcher<Tp> oDataFetcher, Tp buildParameter)
            => new ODataQueryBuilder<T>(
                q => oDataFetcher.FetchData<T>(q, buildParameter),
                q => oDataFetcher.FetchCount<T>(q, buildParameter));

        public static IODataQueryBuilder<T> Create(IODataFetcher oDataFetcher)
            => new ODataQueryBuilder<T>(
                q => oDataFetcher.FetchData<T>(q),
                q => oDataFetcher.FetchCount<T>(q));

        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<IQueryable<T>, IQueryable<T>>> queryExp)
        {
            LambdaExpression cookedQueryExp;
            if (queryExp == null)
            {
                Expression<Func<IQueryable<T>, IQueryable<T>>> takeExp = x => x.Take(ODataConstant.MaxItemSafeGuardCount);
                cookedQueryExp = takeExp;
            }
            else
            {
                var takeSingleItemExp = AppendTake(typeof(T), queryExp.Body, ODataConstant.MaxItemSafeGuardCount);
                cookedQueryExp = Expression.Lambda(takeSingleItemExp, queryExp.Parameters);
            }
            var query = GetDataServiceQuery(cookedQueryExp) as DataServiceRequest;
            var json = await _fetchODataDelegate(query.RequestUri.PathAndQuery);
            return json;
        }

        public async Task<IEnumerable<R>> QueryAsync<R>(Expression<Func<IQueryable<T>, IQueryable<R>>> queryExp)
        {
            var takeSingleItemExp = AppendTake(typeof(T), queryExp.Body, ODataConstant.MaxItemSafeGuardCount);
            var cookedQueryExp = Expression.Lambda(takeSingleItemExp, queryExp.Parameters);
            var query = GetDataServiceQuery(cookedQueryExp) as DataServiceRequest;
            var json = await _fetchODataDelegate(query.RequestUri.PathAndQuery);
            var selectFunc = FindCustomSelectTransform(queryExp.Body as MethodCallExpression);
            return selectFunc(json) as IEnumerable<R>;
        }

        SingleItemResolver<D> GetSingleItemConverter<D>(MethodCallExpression expression)
        {
            return expression.Method.Name switch
            {
                nameof(Queryable.First) => new SingleItemResolver<D>(data => data.First(), 1),
                nameof(Queryable.FirstOrDefault) => new SingleItemResolver<D>(data => data.FirstOrDefault(), 1),
                nameof(Queryable.Single) => new SingleItemResolver<D>(data => data.Single(), 2), //to ensure the set has single item, we have to take 2
                nameof(Queryable.SingleOrDefault) => new SingleItemResolver<D>(data => data.SingleOrDefault(), 2), //to ensure the set has single item, we have to take 2
                _ => throw new JobLogicODataClientException("Invoked Single Item Selector not supported")
            };
        }

        public async Task<T> QueryAsync(Expression<Func<IQueryable<T>, T>> queryExp)
        {
            var methodCallExp = queryExp.Body as MethodCallExpression;
            var singleItemResolver = GetSingleItemConverter<T>(methodCallExp);
            var countArguments = methodCallExp.Arguments.Count;

            if (countArguments == 1)
            {
                var prevExpOfSingleItemExp = methodCallExp.Arguments.First();
                var elementType = prevExpOfSingleItemExp.Type.GetGenericArguments();

                var takeMethodInfo = ReflectionUtils.MethodInfoForQueryableTakeIntCount(elementType.First());

                var takeSingleItemExp = Expression.Call(takeMethodInfo, prevExpOfSingleItemExp, Expression.Constant(singleItemResolver.TakeTop));
                var cookedQueryExp = Expression.Lambda(takeSingleItemExp, queryExp.Parameters);
                var query = GetDataServiceQuery(cookedQueryExp) as DataServiceRequest;
                var json = await _fetchODataDelegate(query.RequestUri.PathAndQuery);
                if (json == null)
                    json = Array.Empty<T>();
                return singleItemResolver.SingleItemConverter(json);
            }
            else if (countArguments == 2)
            {
                var conditionExp = methodCallExp.Arguments.Last();
                var condExp = (conditionExp as UnaryExpression).Operand as Expression<Func<T,bool>>;

                if (condExp != null)
                {
                    var firstArgumentExp = methodCallExp.Arguments.First();
                    var firstArgumentWithWhereExp = AppendWhere(firstArgumentExp, condExp);
                    var elementType = typeof(T);

                    var takeExp = AppendTake(elementType, firstArgumentWithWhereExp, singleItemResolver.TakeTop);
                    var query = GetDataServiceQuery(Expression.Lambda(takeExp, queryExp.Parameters)) as DataServiceRequest;
                    var json = await _fetchODataDelegate(query.RequestUri.PathAndQuery);
                    if (json == null)
                        json = Array.Empty<T>();
                    return singleItemResolver.SingleItemConverter(json);
                }
            }

            throw new JobLogicODataClientException("Expression not supported");
        }

        public async Task<R> QueryAsync<R>(Expression<Func<IQueryable<T>, R>> queryExp)
        {
            var methodCallExp = queryExp.Body as MethodCallExpression;
            var countArguments = methodCallExp.Arguments.Count;
            if (countArguments == 1)
            {
                var prevExpOfSingleItemExp = methodCallExp.Arguments.First() as MethodCallExpression;
                if (methodCallExp.Method.Name == nameof(Queryable.Count) || methodCallExp.Method.Name == nameof(Queryable.LongCount))
                {
                    DataServiceRequest dataServiceRequest;
                    if (prevExpOfSingleItemExp != null)
                    {
                        var takeExp = AppendTake(typeof(T), prevExpOfSingleItemExp, ODataConstant.MaxItemSafeGuardCount);
                        dataServiceRequest = GetDataServiceQuery(Expression.Lambda(takeExp, queryExp.Parameters)) as DataServiceRequest;
                    }
                    else //case when methodCallExp.Arguments is TypedParameterExpression
                    {
                        Expression<Func<IQueryable<T>, IQueryable<T>>> takeExp = x => x.Take(ODataConstant.MaxItemSafeGuardCount);
                        dataServiceRequest = GetDataServiceQuery(takeExp) as DataServiceRequest;
                    }
                    var count = await _fetchCountODataDelegate(dataServiceRequest.RequestUri.PathAndQuery);
                    return (R)Convert.ChangeType(count, typeof(R));
                }
                else if (methodCallExp.Method.Name == nameof(Queryable.Any))
                {
                    DataServiceRequest dataServiceRequest;
                    if (prevExpOfSingleItemExp != null)
                    {
                        var takeExp = AppendTake(typeof(T), prevExpOfSingleItemExp, 1);
                        dataServiceRequest = GetDataServiceQuery(Expression.Lambda(takeExp, queryExp.Parameters)) as DataServiceRequest;
                    }
                    else //case when methodCallExp.Arguments is TypedParameterExpression
                    {
                        Expression<Func<IQueryable<T>, IQueryable<T>>> takeExp = x => x.Take(1);
                        dataServiceRequest = GetDataServiceQuery(takeExp) as DataServiceRequest;
                    }
                    var data = await _fetchODataDelegate(dataServiceRequest.RequestUri.PathAndQuery);
                    return (R)Convert.ChangeType(data.Length, typeof(R));
                }
                else
                {
                    var singleItemResolver = GetSingleItemConverter<R>(methodCallExp);
                    var takeSingleItemExp = AppendTake(typeof(T), prevExpOfSingleItemExp, singleItemResolver.TakeTop);
                    var cookedQueryExp = Expression.Lambda(takeSingleItemExp, queryExp.Parameters);
                    var query = GetDataServiceQuery(cookedQueryExp) as DataServiceRequest;
                    var json = await _fetchODataDelegate(query.RequestUri.PathAndQuery);
                    var selectFunc = FindCustomSelectTransform(queryExp.Body as MethodCallExpression);
                    var data = selectFunc(json) as IEnumerable<R>;
                    if (data == null)
                        data = Enumerable.Empty<R>();
                    return singleItemResolver.SingleItemConverter(data);
                }
            }
            else if (countArguments == 2)
            {
                if (methodCallExp.Method.Name == nameof(Queryable.Any))
                {
                    var conditionExp = methodCallExp.Arguments.Last();
                    var condExp = (conditionExp as UnaryExpression).Operand as Expression<Func<T, bool>>;
                    if (condExp != null)
                    {
                        var firstArgumentExp = methodCallExp.Arguments.First();
                        var firstArgumentWithWhereExp = AppendWhere(firstArgumentExp, condExp);

                        var takeExp = AppendTake(typeof(T), firstArgumentWithWhereExp, 1);
                        var dataServiceRequest = GetDataServiceQuery(Expression.Lambda(takeExp, queryExp.Parameters)) as DataServiceRequest;
                        var data = await _fetchODataDelegate(dataServiceRequest.RequestUri.PathAndQuery);
                        return (R)Convert.ChangeType(data.Length, typeof(R));
                    }
                }
            }

            throw new JobLogicODataClientException("Expression not supported");
        }

        private Expression AppendTake(Type elementType, Expression expression, int takeTop)
        {
            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression != null)
            {
                var argType = methodCallExpression.Type.GetGenericArguments().First();
                if (argType == elementType)
                {
                    var methodInfo = ReflectionUtils.MethodInfoForQueryableTakeIntCount(elementType);
                    var takeExp = Expression.Call(methodInfo, methodCallExpression, Expression.Constant(takeTop));
                    return takeExp;
                }
                else
                {
                    var exp = AppendTake(elementType, methodCallExpression.Arguments.First(), takeTop);
                    var result = Expression.Call(methodCallExpression.Method, exp, methodCallExpression.Arguments.LastOrDefault());
                    return result;
                }
            }
            else
            {
                var methodInfo = ReflectionUtils.MethodInfoForQueryableTakeIntCount(elementType);
                var takeExp = Expression.Call(methodInfo, expression, Expression.Constant(takeTop));
                return takeExp;
            }
        }

        

        private Expression AppendWhere(Expression expression, Expression<Func<T, bool>> whereCondition)
        {
            var elementType = typeof(T);

            var methodInfo = Expression.Call(typeof(Queryable), nameof(Queryable.Where), new Type[] { typeof(T) },
            Expression.Default(typeof(IQueryable<T>)), Expression.Default(typeof(Expression<Func<T, bool>>))).Method;

            var methodCallExpression = expression as MethodCallExpression;

            if (methodCallExpression != null)
            {
                var argType = methodCallExpression.Type.GetGenericArguments().First();
                if (argType == elementType)
                {
                    var whereExp = Expression.Call(methodInfo, methodCallExpression, whereCondition);
                    return whereExp;
                }
                throw new JobLogicODataClientException("Expression not supported");
            }
            else
            {
                var whereExp = Expression.Call(methodInfo, expression, whereCondition);
                return whereExp;
            }
        }

        private Func<T[], object> FindCustomSelectTransform(MethodCallExpression methodCallExpr)
        {
            if (methodCallExpr.Method.Name == nameof(Queryable.Select))
            {
                Func<T[], object> selectFunc = json =>
                {
                    if (json == null) return null;
                    var mappedJson = methodCallExpr.Method.Invoke(null, new object[]
                    {
                        json.AsQueryable(),
                        (methodCallExpr.Arguments.Last() as UnaryExpression).Operand

                    });
                    return mappedJson;
                };
                return selectFunc;
            }
            else if (methodCallExpr.Arguments.Count > 0)
            {
                return FindCustomSelectTransform(methodCallExpr.Arguments[0] as MethodCallExpression);
            }
            throw new JobLogicODataClientException($"Can't find transform select expression: '{methodCallExpr.Method.Name}'");
        }

        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> queryExp)
        {
            var takeSingleItemExp = AppendTake(typeof(T), queryExp.Body, ODataConstant.MaxItemSafeGuardCount);
            var cookedQueryExp = Expression.Lambda(takeSingleItemExp, queryExp.Parameters);

            var query = GetDataServiceQuery(cookedQueryExp) as DataServiceRequest;
            var json = await _fetchODataDelegate(query.RequestUri.PathAndQuery);
            return json;
        }
    }
}
