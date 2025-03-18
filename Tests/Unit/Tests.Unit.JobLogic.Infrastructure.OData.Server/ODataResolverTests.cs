using FluentAssertions;
using JobLogic.Infrastructure.OData.Server;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
//using Microsoft.OData.ModelBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;

namespace Tests.Unit.JobLogic.Infrastructure.OData.Server
{
    [TestClass]
    public class ODataResolverTests
    {

        [TestMethod("ResolveODataQueryOptions | Should Works | When SELECT TOP 1")]
        public async Task Test1()
        {
            //Arrage

            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>();

            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$top=1");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(1);
        }
        
        [TestMethod("ResolveODataQueryOptions | Should Works | When ORDER BY Name")]
        public async Task OrderBy_ByName_ShouldSuccess()
        {
            //Arrage

            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>().ToList();
            for (int i = 0; i < seedModels.Count; i++)
            {
                seedModels[i].Name = $"Model Name {i}";
            }

            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$orderby=Name");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(3);
            data.First().Name.Should().Be("Model Name 0");
            data.Last().Name.Should().Be("Model Name 2");
        }
        
        [TestMethod("ResolveODataQueryOptions | Should Works | When SKIP 5")]
        public async Task Skip_ShouldSuccess()
        {
            //Arrage

            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>(10).ToList();
            for (int i = 0; i < seedModels.Count; i++)
            {
                seedModels[i].Name = $"Model Name {i}";
            }

            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$skip=5");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(5);
        }
    }
}
