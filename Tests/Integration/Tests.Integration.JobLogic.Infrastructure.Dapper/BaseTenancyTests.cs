using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Transactions;

namespace Tests.Integration.JobLogic.Infrastructure.Dapper
{
    [TestCategory("Integration")]
    public abstract class BaseTenancyTests
    {
        private TransactionScope _transactionScope;
        private static ServiceCollection _serviceCollection;

        protected IServiceProvider _serviceProvider;
        protected Random random;
        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void BaseClassInit(TestContext testContext)
        {
            var src = SetupInitializer.TenancyServiceCollection;
            _serviceCollection = new ServiceCollection();
            foreach (var v in src)
            {
                _serviceCollection.Add(v);
            }
        }

        protected static void ModifyServiceCollection(Action<ServiceCollection> servicesModifyAct)
        {
            servicesModifyAct(_serviceCollection);
        }

        [TestInitialize]
        public void InitializeAtBaseLevel()
        {
            _serviceProvider = _serviceCollection.BuildServiceProvider().CreateScope().ServiceProvider;
            _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        protected Ts GetService<Ts>()
        {
            return _serviceProvider.GetService<Ts>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_transactionScope != null)
                _transactionScope.Dispose();
        }
    }
}
