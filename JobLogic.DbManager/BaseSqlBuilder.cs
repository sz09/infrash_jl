using System;
using System.Collections.Generic;
using System.Data;

namespace JobLogic.DatabaseManager
{
    public abstract class BaseSqlBuilder
    {
        int _parameterCount;
        IList<IDbDataParameter> _queryParams;
        string GenerateParameterName() => "JLSQLParam" + _parameterCount++;
        
        protected void AddParameter(string nameWithoutATPrefix, object value)
        {
            _queryParams.Add(value.AsParam(nameWithoutATPrefix));
        }
        protected string Sql(Func<string> sqlFactory)
        {
            return Environment.NewLine + sqlFactory();
        }
        protected string Sql<T>(Func<Func<string>, string> sqlFactory, T value)
        {
            var parameterName = GenerateParameterName();
            bool didGetParameterNameFuncInvoked = false;
            Func<string> GetParameterNameFunc = () =>
            {
                didGetParameterNameFuncInvoked = true;
                return parameterName;
            };
            var result = sqlFactory(GetParameterNameFunc);
            if (didGetParameterNameFuncInvoked)
                _queryParams.Add(value.AsParam(parameterName));
            return Environment.NewLine + result;
        }

        protected string SqlIf(bool cond, string sql)
        {
            return Environment.NewLine + (cond ? sql : string.Empty);
        }
        protected string SqlIf(bool cond, Func<string> sqlFactory)
        {
            return Environment.NewLine + (cond ? sqlFactory() : string.Empty);
        }
        protected string SqlIf<T>(bool cond, Func<Func<string>, string> sqlFactory, T value)
        {
            if (cond)
            {
                var parameterName = GenerateParameterName();
                bool didGetParameterNameFuncInvoked = false;
                Func<string> GetParameterNameFunc = () =>
                {
                    didGetParameterNameFuncInvoked = true;
                    return parameterName;
                };
                var result = sqlFactory(GetParameterNameFunc);
                if (didGetParameterNameFuncInvoked)
                    _queryParams.Add(value.AsParam(parameterName));
                return Environment.NewLine + (cond ? result : string.Empty);
            }
            else return Environment.NewLine;
        }

        protected string SqlRepeat<T>(Func<Func<string>, T, string> sqlFactory, IEnumerable<T> parameters)
        {
            string result = string.Empty;
            foreach (var p in parameters)
            {
                var parameterName = GenerateParameterName();
                bool didGetParameterNameFuncInvoked = false;
                Func<string> GetParameterNameFunc = () =>
                {
                    didGetParameterNameFuncInvoked = true;
                    return parameterName;
                };
                result += sqlFactory(GetParameterNameFunc, p);
                if (didGetParameterNameFuncInvoked)
                    _queryParams.Add(p.AsParam(parameterName));
            }
            return Environment.NewLine + result;
        }
        protected string SqlRepeatIf<T>(bool cond, Func<Func<string>, string> sqlFactory, IEnumerable<T> parameters)
        {
            if (cond)
            {
                string result = string.Empty;
                foreach (var p in parameters)
                {
                    var parameterName = GenerateParameterName();
                    bool didGetParameterNameFuncInvoked = false;
                    Func<string> GetParameterNameFunc = () =>
                    {
                        didGetParameterNameFuncInvoked = true;
                        return parameterName;
                    };
                    result += sqlFactory(GetParameterNameFunc);
                    if (didGetParameterNameFuncInvoked)
                        _queryParams.Add(p.AsParam(parameterName));
                }
                return Environment.NewLine + (cond ? result : string.Empty);
            }
            else return Environment.NewLine;
        }
        protected string SqlRepeatIf<T>(bool cond, Func<Func<string>, T, string> sqlFactory, IEnumerable<T> parameters)
        {
            if (cond)
            {
                string result = string.Empty;
                foreach (var p in parameters)
                {
                    var parameterName = GenerateParameterName();
                    bool didGetParameterNameFuncInvoked = false;
                    Func<string> GetParameterNameFunc = () =>
                    {
                        didGetParameterNameFuncInvoked = true;
                        return parameterName;
                    };
                    result += sqlFactory(GetParameterNameFunc, p);
                    if (didGetParameterNameFuncInvoked)
                        _queryParams.Add(p.AsParam(parameterName));
                }
                return Environment.NewLine + (cond ? result : string.Empty);
            }
            else return Environment.NewLine;
        }

        protected string SqlImplodeOr<T>(Func<Func<string>, string> sqlFactory, IEnumerable<T> parameters)
        {
            IList<string> result = new List<string>();
            foreach (var p in parameters)
            {
                var parameterName = GenerateParameterName();
                bool didGetParameterNameFuncInvoked = false;
                Func<string> GetParameterNameFunc = () =>
                {
                    didGetParameterNameFuncInvoked = true;
                    return parameterName;
                };
                result.Add(sqlFactory(GetParameterNameFunc));
                if (didGetParameterNameFuncInvoked)
                    _queryParams.Add(p.AsParam(parameterName));
            }
            return Environment.NewLine + string.Join(" Or ", result);
        }
        protected string SqlImplodeOr<T>(Func<Func<string>, T, string> sqlFactory, IEnumerable<T> parameters)
        {
            return SqlImplode(" Or ", sqlFactory, parameters);
        }
        protected string SqlImplodeComa<T>(Func<Func<string>, T, string> sqlFactory, IEnumerable<T> parameters)
        {
            return SqlImplode(" , ", sqlFactory, parameters);
        }

        private string SqlImplode<T>(string implodeVal,Func<Func<string>, T, string> sqlFactory, IEnumerable<T> parameters)
        {
            IList<string> result = new List<string>();
            foreach (var p in parameters)
            {
                var parameterName = GenerateParameterName();
                bool didGetParameterNameFuncInvoked = false;
                Func<string> GetParameterNameFunc = () =>
                {
                    didGetParameterNameFuncInvoked = true;
                    return parameterName;
                };
                result.Add(sqlFactory(GetParameterNameFunc, p));
                if (didGetParameterNameFuncInvoked)
                    _queryParams.Add(p.AsParam(parameterName));
            }
            return Environment.NewLine + string.Join(implodeVal, result);
        }

        protected abstract string SqlFinalQueryString();

        public SqlQuery CreateSqlQuery()
        {
            _parameterCount = 0;
            _queryParams = new List<IDbDataParameter>();
            var sql = SqlFinalQueryString();
            return new SqlQuery
            {
                QueryString = sql,
                QueryParams = _queryParams
            };
        }
    }
}
