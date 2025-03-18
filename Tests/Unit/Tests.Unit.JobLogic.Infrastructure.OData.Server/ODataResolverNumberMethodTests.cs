using FluentAssertions;
using JobLogic.Infrastructure.OData.Server;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.AspNet.OData.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
//using Microsoft.OData.ModelBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.OData.Server
{
    [TestClass]
    public class ODataResolverNumberMethodTests
    {

        [TestMethod("ResolveODataQueryOptions | Should Works | When Greater Than")]
        public async Task GreaterThan_ShouldSuccess()
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

            var seedModels = ValueGenerator.BuildObject<Model>().With(d => d.Age, 15).CreateMany(3).ToList();
            var seedModels1 = ValueGenerator.CreateMany<Model>(7).ToList();
            foreach (var item in seedModels1)
            {
                item.Age = rdm.Next(19, 50);
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Age gt 18");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(7);
            foreach (var item in data)
            {
                item.Age.Should().BeGreaterThan(18);
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Greater Or Equal")]
        public async Task GreaterOrEqual_ShouldSuccess()
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

            var seedModels = ValueGenerator.BuildObject<Model>().With(d => d.Age, 15).CreateMany(3).ToList();
            var seedModels1 = ValueGenerator.CreateMany<Model>(7).ToList();
            foreach (var item in seedModels1)
            {
                item.Age = rdm.Next(18, 50);
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Age ge 18");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(7);
            foreach (var item in data)
            {
                item.Age.Should().BeGreaterThanOrEqualTo(18);
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Less Than")]
        public async Task LessThan_ShouldSuccess()
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

            var seedModels = ValueGenerator.CreateMany<Model>(3).ToList();
            var seedModels1 = ValueGenerator.BuildObject<Model>().With(d => d.Age, 75).CreateMany(3).ToList();
            foreach (var item in seedModels)
            {
                item.Age = rdm.Next(18, 50);
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Age lt 55");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(3);
            foreach (var item in data)
            {
                item.Age.Should().BeLessThan(55);
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Less Than Or Equal")]
        public async Task LessThanOrEqualTo_ShouldSuccess()
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

            var seedModels = ValueGenerator.CreateMany<Model>(3).ToList();
            var seedModels1 = ValueGenerator.BuildObject<Model>().With(d => d.Age, 75).CreateMany(3).ToList();
            foreach (var item in seedModels)
            {
                item.Age = rdm.Next(18, 50);
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Age le 50");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(3);
            foreach (var item in data)
            {
                item.Age.Should().BeLessThanOrEqualTo(50);
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Add")]
        public async Task Add_ShouldSuccess()
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
                item.Age = rdm.Next(23, 27);
            }
            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Age add 5 gt 30");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(seedModels.Where(d => d.Age > 25).Count());
            foreach (var item in data)
            {
                item.Age.Should().BeGreaterThan(25);
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Sub")]
        public async Task Sub_ShouldSuccess()
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
                item.Age = rdm.Next(23, 27);
            }
            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Age sub 5 lt 20");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(seedModels.Where(d => d.Age < 25).Count());
            foreach (var item in data)
            {
                item.Age.Should().BeLessThan(25);
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Mul")]
        public async Task Mul_ShouldSuccess()
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
                item.Age = rdm.Next(23, 27);
            }
            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Age mul 10 lt 250");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(seedModels.Where(d => d.Age < 25).Count());
            foreach (var item in data)
            {
                item.Age.Should().BeLessThan(25);
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Div")]
        public async Task Div_ShouldSuccess()
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
                item.Age = rdm.Next(23, 57);
            }
            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Age div 5 lt 8");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(seedModels.Where(d => d.Age < 40).Count());
            foreach (var item in data)
            {
                item.Age.Should().BeLessThan(40);
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When Mod")]
        public async Task Mod_ShouldSuccess()
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
                item.Age = rdm.Next(23, 57);
            }
            context.AddRange(seedModels);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Age mod 2 eq 0");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(seedModels.Where(d => d.Age % 2 == 0).Count());
            foreach (var item in data)
            {
                (item.Age % 2).Should().Be(0);
            }
        }
    }
}
