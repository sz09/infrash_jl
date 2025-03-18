using Microsoft.OData.Client;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace JobLogic.Infrastructure.OData.Client
{
    public static class ODataExtension
    {
        public static IQueryable<T> Expand<T, TTarget>(this IQueryable<T> q,Expression<Func<T, TTarget>> navigationPropertyAccessor)
        {
            var dataservicequery = q as DataServiceQuery<T>;
            if (dataservicequery == null)
                throw new JobLogicODataClientException("Expand method require DataServiceQuery<T> instance");
            return dataservicequery.Expand(navigationPropertyAccessor);
        }
    }
}
