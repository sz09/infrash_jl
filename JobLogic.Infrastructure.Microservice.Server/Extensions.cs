using JobLogic.Infrastructure.Microservice.Client;
using Microsoft.Extensions.DependencyInjection;

namespace JobLogic.Infrastructure.Microservice.Server
{
    static class Extensions
    {
        public static void SetScopedResolver4ITenantlessOperationInfo(this IServiceScope scope, ITenantlessOperationInfo value)
        {
            var scopedResolver = scope.ServiceProvider.GetRequiredService<RuntimeScopedServiceResolver<ITenantlessOperationInfo>>();
            scopedResolver.Value = value;
        }

        public static void SetScopedResolver4ITenancyOperationInfo(this IServiceScope scope, ITenancyOperationInfo value)
        {
            var scopedResolver = scope.ServiceProvider.GetRequiredService<RuntimeScopedServiceResolver<ITenancyOperationInfo>>();
            scopedResolver.Value = value;
        }
    }
}
