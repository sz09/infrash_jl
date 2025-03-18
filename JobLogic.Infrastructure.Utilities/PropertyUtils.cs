using System;
using System.Linq.Expressions;

namespace JobLogic.Infrastructure.Utilities
{
    public class PropertyUtils
    {
        private static readonly Lazy<PropertyUtils> lazy =
        new Lazy<PropertyUtils>(() => new PropertyUtils());

        public static PropertyUtils Instance { get { return lazy.Value; } }

        private PropertyUtils()
        {
        }

        public string GetName<T>(Expression<Func<T>> propertyLambda)
        {
            if (propertyLambda != null)
            {
                var memberExpression = propertyLambda.Body as MemberExpression;

                if (memberExpression != null)
                {
                    return memberExpression.Member.Name;
                }
            }

            return string.Empty;
        }
    }
}
