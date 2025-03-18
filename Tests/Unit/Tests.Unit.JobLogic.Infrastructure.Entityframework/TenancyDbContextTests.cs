using JobLogic.Infrastructure.Entityframework;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Entityframework
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
