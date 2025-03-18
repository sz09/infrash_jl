using JobLogic.Infrastructure.Microservice.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tests.Integration.JobLogic.Infrastructure.Dapper;

namespace Tests.Integration.JobLogic.Infrastructure.Dapper
{
    [TestClass]
    public class SetupInitializer
    {
        public static ServiceCollection TenancyServiceCollection { get; private set; }
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("testappsettings.json", optional: true)
                .AddJsonFile($"testappsettings.{Environment.GetEnvironmentVariable("JLTEST_ENV")}.json", optional: true)
                .Build();

            var services = new ServiceCollection();

            services.Configure<AppConfig>(configuration);
            Func<IServiceProvider, AppConfig> appConfigFunc = x => x.GetRequiredService<IOptionsSnapshot<AppConfig>>().Value;

            
            services.AddScoped(sp =>
            {
                var tenantId = Guid.NewGuid();
                return OperationInfoFactory.CreateTenancy(tenantId);
            });
            services.AddTransient(sp =>
            {
                var config = appConfigFunc(sp);
                var opInfo = sp.GetService<ITenancyOperationInfo>();
                return new TenancyDapper(config.JobLogicConnection, opInfo.TenantId);
            });
            TenancyServiceCollection = services;
        }

    }
}
