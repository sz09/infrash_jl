namespace Tests.Integration.JobLogic.DatabaseManager
{
    using global::JobLogic.DatabaseManager;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Configuration;
    using System.Transactions;

    [TestCategory("Integration")]
    public class BaseTest : IDisposable
    {
        private TransactionScope transactionScope;
        protected DbExecutionManager dbExecutionManager;

        [TestInitialize]
        public void Init()
        {
            transactionScope = new TransactionScope();
            var connectionString = ConfigurationManager.ConnectionStrings["LicenseConnection"].ConnectionString;
            var tenancyProvider = new Mock<ITenancyProvider>();
            tenancyProvider.Setup(x => x.ConnectionString).Returns(connectionString);
            tenancyProvider.Setup(x => x.TenantId).Returns(null);

            dbExecutionManager = new DbExecutionManager(tenancyProvider.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transactionScope.Dispose();
        }

        public void Dispose()
        {
            if(transactionScope != null)
            {
                transactionScope.Dispose();
                transactionScope = null;
            }

            dbExecutionManager = null;
        }
    }
}
