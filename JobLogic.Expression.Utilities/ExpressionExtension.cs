using System.Reflection;

namespace JobLogic.Expression.Utilities
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public static class ExpressionExtension
    {
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {

            // build parameter map (from parameters of second to parameters of first)

            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first

            var secondBody = new ParameterRebinder(map).ReplaceParameters(second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
        
        private static MethodInfo GetLikeMethod(string value, char wildcard)
        {
            var methodName = "Equals";

            var textLength = value.Length;
            value = value.TrimEnd(wildcard);
            if (textLength > value.Length)
            {
                methodName = "StartsWith";
                textLength = value.Length;
            }

            value = value.TrimStart(wildcard);
            if (textLength > value.Length)
            {
                methodName = (methodName == "StartsWith") ? "Contains" : "EndsWith";
            }

            var stringType = typeof(string);
            return stringType.GetMethod(methodName, new[] { stringType });
        }
        
        public static Expression<Func<T, bool>> WildCard<T>(Expression<Func<T, string>> valueSelector, string value, char wildcard)
        {
            if (valueSelector == null) throw new ArgumentNullException("valueSelector");

            var method = GetLikeMethod(value, wildcard);

            value = value.Trim(wildcard);
            var body = Expression.Call(valueSelector.Body, method, Expression.Constant(value));

            var parameter = valueSelector.Parameters.Single();
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);

        }
    }
}
