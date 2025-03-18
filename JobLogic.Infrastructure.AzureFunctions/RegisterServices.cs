using Microsoft.Extensions.DependencyInjection;

namespace JobLogic.Infrastructure.AzureFunctions
{
    public static class RegisterServices
    {
        public static void RegisterScopedServiceResolvers(this IServiceCollection services)
        {
            services.AddScoped<ScopedServiceResolver<IJlConnStrResolver>>();
            services.AddScoped(x => x.GetRequiredService<ScopedServiceResolver<IJlConnStrResolver>>().Value);
            services.AddScoped<ScopedServiceResolver<ITenantIdResolver>>();
            services.AddScoped(x => x.GetRequiredService<ScopedServiceResolver<ITenantIdResolver>>().Value);
            services.AddScoped<ScopedServiceResolver<IJobLogicCultureInfoResolver>>();
            services.AddScoped(x => x.GetRequiredService<ScopedServiceResolver<IJobLogicCultureInfoResolver>>().Value);
            services.AddScoped<ScopedServiceResolver<IUserIdResolver>>();
            services.AddScoped(x => x.GetRequiredService<ScopedServiceResolver<IUserIdResolver>>().Value);
        }
    }
}
