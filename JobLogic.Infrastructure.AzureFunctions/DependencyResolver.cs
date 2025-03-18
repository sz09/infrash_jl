using Microsoft.Extensions.DependencyInjection;
using System;

namespace JobLogic.Infrastructure.AzureFunctions
{
    public static class DependencyResolver
    {
        readonly static object _lockObject = new object();

        public static T ResolvePerTenant<T>(this IServiceScope serviceScope, Guid tenantId)
        {
            lock (_lockObject)
            {
                var serviceProvider = serviceScope.ServiceProvider;

                var tenantIdResolver = serviceProvider.GetRequiredService<ScopedServiceResolver<ITenantIdResolver>>();
                tenantIdResolver.Value = new TenantIdResolver { TenantId = tenantId };

                var service = serviceProvider.GetService<T>();
                return service;
            }
        }

        public static T ResolvePerTenant<T>(this IServiceScope serviceScope, CompanyResolver companyResolver)
        {
            lock (_lockObject)
            {
                var serviceProvider = serviceScope.ServiceProvider;

                var tenantIdResolver = serviceProvider.GetRequiredService<ScopedServiceResolver<ITenantIdResolver>>();
                tenantIdResolver.Value = companyResolver;
                var connStrResolver = serviceProvider.GetRequiredService<ScopedServiceResolver<IJlConnStrResolver>>();
                connStrResolver.Value = companyResolver;
                var cultureInfoResolver = serviceProvider.GetRequiredService<ScopedServiceResolver<IJobLogicCultureInfoResolver>>();
                cultureInfoResolver.Value = companyResolver;
                var userInfoResolver = serviceProvider.GetRequiredService<ScopedServiceResolver<IUserIdResolver>>();
                userInfoResolver.Value = companyResolver;
                var service = serviceProvider.GetService<T>();
                return service;
            }
        }
    }
}
