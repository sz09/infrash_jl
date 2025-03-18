using JobLogic.Infrastructure.Microservice.Client;
using Microsoft.Extensions.DependencyInjection;

namespace JobLogic.Infrastructure.Microservice.Server
{
    public static class IoCExtensions
    {
        public static void AddJicroserviceServer(this IServiceCollection services, HandlerConfig tenancyHandlerProviderInfo, 
            HandlerConfig tenantlessHandlerProviderInfo)
        {
            services.AddScoped<RuntimeScopedServiceResolver<ITenancyOperationInfo>>();
            services.AddScoped(x => x.GetRequiredService<RuntimeScopedServiceResolver<ITenancyOperationInfo>>().Value);
            
            services.AddScoped<RuntimeScopedServiceResolver<ITenantlessOperationInfo>>();
            services.AddScoped(x => x.GetRequiredService<RuntimeScopedServiceResolver<ITenantlessOperationInfo>>().Value);

            services.AddScoped<RuntimeScopedServiceResolver<IMiddlewareContextDataReader>>();
            services.AddScoped(x => x.GetRequiredService<RuntimeScopedServiceResolver<IMiddlewareContextDataReader>>().Value);

            services.AddSingleton<IDispatcher, Dispatcher>();
            services.AddSingleton<IHandlerProvider>(x => new HandlerProvider(tenancyHandlerProviderInfo, tenantlessHandlerProviderInfo));
        }
    }
}
