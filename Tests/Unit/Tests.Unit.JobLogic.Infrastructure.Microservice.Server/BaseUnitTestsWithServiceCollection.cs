using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Server
{
    [TestCategory("Unit")]
    public abstract class BaseUnitTestsWithServiceCollection
    {
        protected IServiceCollection _serviceCollection;
        protected abstract void RegisterServices(IServiceCollection services);

        [TestInitialize]
        public void TestInitialize()
        {
            _serviceCollection = new ServiceCollection();
            RegisterServices(_serviceCollection);
        }

        protected Ts GetService<Ts>(params (Type type, object value)[] rebinds)
        {
            foreach (var i in rebinds)
            {
                _serviceCollection.AddTransient(i.type, x => i.value);
            }
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<Ts>();
            return service;
        }

        protected ServiceProvider GetServiceProvider(params (Type type, object value)[] rebinds)
        {
            foreach (var i in rebinds)
            {
                _serviceCollection.AddTransient(i.type, x => i.value);
            }
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
