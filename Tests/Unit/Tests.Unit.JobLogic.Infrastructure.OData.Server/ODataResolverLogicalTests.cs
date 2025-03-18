using FluentAssertions;
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
    public class ODataResolverLogicalTests
    {

        [TestMethod("ResolveODataQueryOptions | Should Works | When And")]
        public async Task And_ShouldSuccess()
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

            var seedModels = ValueGenerator.CreateMany<Model>(10).ToList();
            foreach (var item in seedModels)
            {
                item.Age = rdm.Next(19, 50);
            }

            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Age gt 18 and Age lt 40");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(seedModels.Where(d => d.Age > 18 && d.Age < 40).Count());
            foreach (var item in data)
            {
                item.Age.Should().BeGreaterThan(18);
                item.Age.Should().BeLessThan(40);
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When And Multiple Field")]
        public async Task And_MultipleField_ShouldSuccess()
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

            var seedModels = ValueGenerator.CreateMany<Model>(10).ToList();
            foreach (var item in seedModels)
            {
                item.Age = rdm.Next(19, 50);
                item.ExpYears = rdm.Next(1, 30);
            }

            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Age gt 18 and ExpYears gt 4");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(seedModels.Where(d => d.Age > 18 && d.ExpYears > 4).Count());
            foreach (var item in data)
            {
                item.Age.Should().BeGreaterThan(18);
                item.ExpYears.Should().BeGreaterThan(4);
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Or")]
        public async Task Or_ShouldSuccess()
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

            var seedModels = ValueGenerator.CreateMany<Model>(10).ToList();
            foreach (var item in seedModels)
            {
                item.Age = rdm.Next(19, 50);
                item.ExpYears = rdm.Next(1, 30);
            }

            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Age gt 24 or ExpYears gt 4");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(seedModels.Where(d => d.Age > 24 || d.ExpYears > 4).Count());
            foreach (var item in data)
            {
                (item.Age > 24 || item.ExpYears > 4).Should().BeTrue();
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Or Multiple Field")]
        public async Task Or_MultipleField_ShouldSuccess()
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

            var seedModels = ValueGenerator.CreateMany<Model>(10).ToList();
            foreach (var item in seedModels)
            {
                item.Age = rdm.Next(19, 50);
                item.ExpYears = rdm.Next(1, 30);
            }

            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Age gt 18 and ExpYears gt 4");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(seedModels.Where(d => d.Age > 18 && d.ExpYears > 4).Count());
            foreach (var item in data)
            {
                item.Age.Should().BeGreaterThan(18);
                item.ExpYears.Should().BeGreaterThan(4);
            }
        }
    }
}
