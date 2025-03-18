using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.Microservice.Server;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Server
{
    public static class IoCExtensions
    {
        public static void UseTestServices(this IServiceCollection services, Action<MiddlewareBuilder> tenancyBuildAction = null, Action<MiddlewareBuilder> tenantlessBuildAction = null)
        {
            services.AddJicroserviceServer(
                new HandlerConfig(typeof(IoCExtensions).Assembly, tenancyBuildAction), 
                new HandlerConfig(typeof(IoCExtensions).Assembly, tenantlessBuildAction));

            services.AddScoped<ScopedInnerMostService<ITenancyOperationInfo>>();
            services.AddTransient<Service<ITenancyOperationInfo>>();
            services.AddTransient<InnerService1<ITenancyOperationInfo>>();
            services.AddTransient<InnerService2<ITenancyOperationInfo>>();
            services.AddTransient<InnerInnerService1<ITenancyOperationInfo>>();
            services.AddTransient<InnerInnerService2<ITenancyOperationInfo>>();

            
            services.AddScoped<ScopedInnerMostService<ITenantlessOperationInfo>>();
            services.AddTransient<Service<ITenantlessOperationInfo>>();
            services.AddTransient<InnerService1<ITenantlessOperationInfo>>();
            services.AddTransient<InnerService2<ITenantlessOperationInfo>>();
            services.AddTransient<InnerInnerService1<ITenantlessOperationInfo>>();
            services.AddTransient<InnerInnerService2<ITenantlessOperationInfo>>();

            var tenantAId = Guid.Parse("ce11b023-1a2b-47fd-94fd-68a3cb5abd60");

            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IoCExtensions))
                .AddClasses(x => x.AssignableTo(typeof(IHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(x => x.AssignableTo(typeof(IHandler<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                );
        }

        public static void UseTenantlessOnlyTestServices(this IServiceCollection services, Action<MiddlewareBuilder> tenantlessBuildAction = null)
        {
            services.AddJicroserviceServer(null, new HandlerConfig(typeof(IoCExtensions).Assembly, tenantlessBuildAction));


            services.AddScoped<ScopedInnerMostService<ITenantlessOperationInfo>>();
            services.AddTransient<Service<ITenantlessOperationInfo>>();
            services.AddTransient<InnerService1<ITenantlessOperationInfo>>();
            services.AddTransient<InnerService2<ITenantlessOperationInfo>>();
            services.AddTransient<InnerInnerService1<ITenantlessOperationInfo>>();
            services.AddTransient<InnerInnerService2<ITenantlessOperationInfo>>();

            var tenantAId = Guid.Parse("ce11b023-1a2b-47fd-94fd-68a3cb5abd60");

            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IoCExtensions))
                .AddClasses(x => x.AssignableTo(typeof(IHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(x => x.AssignableTo(typeof(IHandler<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                );
        }

        public static void UseTenancyOnlyTestServices(this IServiceCollection services, Action<MiddlewareBuilder> tenancyBuildAction = null)
        {
            services.AddJicroserviceServer(new HandlerConfig(typeof(IoCExtensions).Assembly, tenancyBuildAction),null);

            services.AddScoped<ScopedInnerMostService<ITenancyOperationInfo>>();
            services.AddTransient<Service<ITenancyOperationInfo>>();
            services.AddTransient<InnerService1<ITenancyOperationInfo>>();
            services.AddTransient<InnerService2<ITenancyOperationInfo>>();
            services.AddTransient<InnerInnerService1<ITenancyOperationInfo>>();
            services.AddTransient<InnerInnerService2<ITenancyOperationInfo>>();

            var tenantAId = Guid.Parse("ce11b023-1a2b-47fd-94fd-68a3cb5abd60");

            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IoCExtensions))
                .AddClasses(x => x.AssignableTo(typeof(IHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(x => x.AssignableTo(typeof(IHandler<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                );
        }
    }
}
