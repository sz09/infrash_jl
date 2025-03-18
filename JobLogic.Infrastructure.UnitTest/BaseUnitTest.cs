using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.MockingKernel.Moq;

namespace JobLogic.Infrastructure.UnitTest
{
    [TestCategory("Unit")]
    public abstract class BaseUnitTest : BaseTest
    {
        protected MoqMockingKernel _mockingKernel { get; private set; }

        [TestInitialize]
        public void BaseUnitTestInitialize()
        {
            _mockingKernel = new MoqMockingKernel();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockingKernel.Dispose();
        }
    }
}
