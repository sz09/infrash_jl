using System;
using System.Linq;
using System.Linq.Expressions;

namespace JobLogic.Infrastructure.ChangeRecorder
{
    static class LambdaExpressionExtensions
    {
        public static string GetChangeRecordDictKey(this LambdaExpression lambdaExpression)
        {
            var memExp = lambdaExpression.Body as MemberExpression;
            var paramName = lambdaExpression.Parameters.Single().Name;
            var expString = memExp.ToString();
            var accessParamPart = paramName + ".";
            if (expString.StartsWith(accessParamPart))
            {
                expString = expString.Substring(accessParamPart.Length);
            }
            else
            {
                throw new InvalidOperationException("Invalid MemberExpression for GetChangeRecordDictKey()");
            }
            return expString;
        }
    }
}
