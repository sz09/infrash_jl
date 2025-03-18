using JobLogic.Infrastructure.EntityframeworkCore;
using JobLogic.Infrastructure.UnitTest.EFCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Unit.JobLogic.Infrastructure.EntityframeworkCore
{
    [TestClass]
    public class TenancyDbContextTests
    {
        [TestMethod]
        public async Task TestSaveChangesAsync_ShouldHasNoException()
        {
            var ctx = new TestDBContext(ValueGenerator.String(), Guid.Empty);
            await ctx.SaveChangesAsync();
        }

        [TestMethod]
        public void TestSaveChanges_ShouldHasNoException()
        {
            var ctx = new TestDBContext(ValueGenerator.String(), Guid.Empty);
            ctx.SaveChanges();
        }

        [TestMethod]
        public async Task TestSaveChangesAsync_ShouldHasNoException_WhenHasTokenCancellation()
        {
            var ctx = new TestDBContext(ValueGenerator.String(), Guid.Empty);
            await ctx.SaveChangesAsync(new System.Threading.CancellationToken());
        }
    }

    public class TestDBContext : TenancyDbContext
    {
        public TestDBContext(string connectionString, Guid tenantId) : base(connectionString, tenantId)
        {
        }
    }
}
