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
    public class ODataResolverStringMethodTests
    {

        [TestMethod("ResolveODataQueryOptions | Should Works | When FILTER Equal")]
        public async Task Equal_ShouldSuccess()
        {
            //Arrage

            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>(5).ToList();
            var seedModels1 = ValueGenerator.CreateMany<Model>(5).ToList();
            foreach (var item in seedModels1)
            {
                item.Name = $"Model Name";
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Name eq 'Model Name'");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(5);
            foreach (var item in data)
            {
                item.Name.Should().Be("Model Name");
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When FILTER Not Equal")]
        public async Task NotEqual_ShouldSuccess()
        {
            //Arrage

            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>(5).ToList();
            var seedModels1 = ValueGenerator.CreateMany<Model>(5).ToList();
            foreach (var item in seedModels1)
            {
                item.Name = $"Model Name";
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Name ne 'Model Name'");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(5);
            foreach (var item in data)
            {
                item.Name.Should().NotBe("Model Name");
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When FILTER StartsWith")]
        public async Task Filter_StartsWith_ShouldSuccess()
        {
            //Arrage

            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>(5).ToList();
            var seedModels1 = ValueGenerator.CreateMany<Model>(5).ToList();
            foreach (var item in seedModels1)
            {
                item.Name = $"Model Name {ValueGenerator.String()}";
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=startswith(Name, 'Model Name') eq true");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(5);
            foreach (var item in data)
            {
                item.Name.Should().StartWith("Model Name");
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When FILTER Not StartsWith")]
        public async Task Filter_NotStartsWith_ShouldSuccess()
        {
            //Arrage

            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>(3).ToList();
            var seedModels1 = ValueGenerator.CreateMany<Model>(5).ToList();
            foreach (var item in seedModels1)
            {
                item.Name = $"Model Name {ValueGenerator.String()}";
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=not startswith(Name, 'Model Name') eq true");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(3);
            foreach (var item in data)
            {
                item.Name.Should().NotStartWith("Model Name");
            }
        }
        
        [TestMethod("ResolveODataQueryOptions | Should Works | When FILTER EndsWith")]
        public async Task Filter_EndsWith_ShouldSuccess()
        {
            //Arrage

            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>(5).ToList();
            var seedModels1 = ValueGenerator.CreateMany<Model>(5).ToList();
            foreach (var item in seedModels1)
            {
                item.Name = $"{ValueGenerator.String()} Model Name";
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=endswith(Name, 'Model Name') eq true");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(5);
            foreach (var item in data)
            {
                item.Name.Should().EndWith("Model Name");
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When FILTER Not EndsWith")]
        public async Task Filter_NotEndsWith_ShouldSuccess()
        {
            //Arrage

            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>(3).ToList();
            var seedModels1 = ValueGenerator.CreateMany<Model>(5).ToList();
            foreach (var item in seedModels1)
            {
                item.Name = $"{ValueGenerator.String()} Model Name";
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=not endswith(Name, 'Model Name') eq true");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(3);
            foreach (var item in data)
            {
                item.Name.Should().NotEndWith("Model Name");
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When FILTER IndexOf")]
        public async Task Filter_IndexOf_ShouldSuccess()
        {
            //Arrage

            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>(5).ToList();
            var seedModels1 = ValueGenerator.CreateMany<Model>(5).ToList();
            foreach (var item in seedModels1)
            {
                item.Name = $"{ValueGenerator.String()} Model Name {ValueGenerator.String()}";
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=indexof(Name, 'Model Name') gt -1");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(5);
            foreach (var item in data)
            {
                item.Name.Should().Contain("Model Name");
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When FILTER ToLower")]
        public async Task Filter_ToLower_ShouldSuccess()
        {
            //Arrage
            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>(5).ToList();
            var seedModels1 = ValueGenerator.CreateMany<Model>(5).ToList();
            foreach (var item in seedModels1)
            {
                item.Name = $"Model Name";
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=tolower(Name) eq 'model name'");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(5);
            foreach (var item in data)
            {
                item.Name.Should().Be("Model Name");
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When FILTER ToUpper")]
        public async Task Filter_ToUpper_ShouldSuccess()
        {
            //Arrage
            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>(5).ToList();
            var seedModels1 = ValueGenerator.CreateMany<Model>(5).ToList();
            foreach (var item in seedModels1)
            {
                item.Name = $"Model Name";
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=toupper(Name) eq 'MODEL NAME'");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(5);
            foreach (var item in data)
            {
                item.Name.Should().Be("Model Name");
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When FILTER Substring")]
        public async Task Filter_Substring_ShouldSuccess()
        {
            //Arrage
            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>(5).ToList();
            var seedModels1 = ValueGenerator.CreateMany<Model>(5).ToList();
            foreach (var item in seedModels1)
            {
                item.Name = $"Model Name";
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=substring(Name, 2) eq 'del Name'");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(5);
            foreach (var item in data)
            {
                item.Name.Should().Be("Model Name");
            }
        }

        [TestMethod("ResolveODataQueryOptions | Should Works | When FILTER Substring 1")]
        public async Task Filter_Substring1_ShouldSuccess()
        {
            //Arrage
            var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(nameof(TestDbContext))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new TestDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var seedModels = ValueGenerator.CreateMany<Model>(5).ToList();
            var seedModels1 = ValueGenerator.CreateMany<Model>(5).ToList();
            foreach (var item in seedModels1)
            {
                item.Name = $"Model Name";
            }

            context.AddRange(seedModels);
            context.AddRange(seedModels1);

            context.SaveChanges();

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Model>(nameof(Model));
            var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

            //Action
            var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=substring(Name, 2, 2) eq 'de'");
            var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
            var data = await rs.ToListAsync();
            //Assert
            data.Count.Should().Be(5);
            foreach (var item in data)
            {
                item.Name.Should().Be("Model Name");
            }
        }
    }
}
