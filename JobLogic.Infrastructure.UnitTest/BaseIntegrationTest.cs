using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;

namespace JobLogic.Infrastructure.UnitTest
{
    [TestCategory("Integration")]
    public abstract class BaseIntegrationTest : BaseTest
    {
        private TransactionScope transactionScope;

        [TestInitialize]
        public void BaseUnitTestInitialize()
        {
            transactionScope = new TransactionScope();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transactionScope.Dispose();
        }

        public void Dispose()
        {
            if (transactionScope != null)
            {
                transactionScope.Dispose();
                transactionScope = null;
            }
        }
    }
}
