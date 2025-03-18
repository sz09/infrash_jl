using JobLogic.Infrastructure.OData.Server;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
//using Microsoft.OData.ModelBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;

namespace Tests.Unit.JobLogic.Infrastructure.OData.Server
{
    [TestClass]
    public class ODataResolverDateTimeMethodTests
    {

        [TestMethod("ResolveODataQueryOptions | Should Works | When Get Year")]
        public async Task GetYear_ShouldSuccess()
        {
            //Arrage
            Random rdm = new Random();
            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.BuildObject<Model>().CreateMany(10).ToList();
            int index = rdm.Next(10);
            seedModels[index].Birthday = new DateTime(2022, 2, 12, 10, 15, 35);

            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=year(Birthday) eq 2022");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            Assert.IsTrue(data.Any(d => d.Id == seedModels[index].Id));
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Get Month")]
        public async Task GetMonth_ShouldSuccess()
        {
            //Arrage
            Random rdm = new Random();
            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.BuildObject<Model>().CreateMany(10).ToList();
            int index = rdm.Next(10);
            seedModels[index].Birthday = new DateTime(2022, 2, 12, 10, 15, 35);

            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=month(Birthday) eq 2");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            Assert.IsTrue(data.Any(d => d.Id == seedModels[index].Id));
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Get Day")]
        public async Task GetDay_ShouldSuccess()
        {
            //Arrage
            Random rdm = new Random();
            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.BuildObject<Model>().CreateMany(10).ToList();
            int index = rdm.Next(10);
            seedModels[index].Birthday = new DateTime(2022, 2, 12, 10, 15, 35);

            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=day(Birthday) eq 12");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            Assert.IsTrue(data.Any(d => d.Id == seedModels[index].Id));
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Get Hour")]
        public async Task GetHour_ShouldSuccess()
        {
            //Arrage
            Random rdm = new Random();
            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.BuildObject<Model>().CreateMany(10).ToList();
            int index = rdm.Next(10);
            seedModels[index].Birthday = new DateTime(2022, 2, 12, 10, 15, 35);

            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=hour(Birthday) eq 10");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            Assert.IsTrue(data.Any(d => d.Id == seedModels[index].Id));
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Get Minute")]
        public async Task GetMinute_ShouldSuccess()
        {
            //Arrage
            Random rdm = new Random();
            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.BuildObject<Model>().CreateMany(10).ToList();
            int index = rdm.Next(10);
            seedModels[index].Birthday = new DateTime(2022, 2, 12, 10, 15, 35);

            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=minute(Birthday) eq 15");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            Assert.IsTrue(data.Any(d => d.Id == seedModels[index].Id));
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Get Second")]
        public async Task GetSecond_ShouldSuccess()
        {
            //Arrage
            Random rdm = new Random();
            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.BuildObject<Model>().CreateMany(10).ToList();
            int index = rdm.Next(10);
            seedModels[index].Birthday = new DateTime(2022, 2, 12, 10, 15, 35);

            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=second(Birthday) eq 35");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            Assert.IsTrue(data.Any(d => d.Id == seedModels[index].Id));
        }

    }
}
