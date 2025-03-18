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
    public class ODataResolverNavigationTests
    {

        [TestMethod("ResolveODataQueryOptions | Should Works | When Navigation Model")]
        public async Task NavigationModel_ShouldSuccess()
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
            var navigationModel = ValueGenerator.CreateObject<Navigation_Model>();
            context.Add(navigationModel);

            var seedModels = ValueGenerator.BuildObject<Model>().With(d => d.Navigation_Model_Id, navigationModel.Id).CreateMany(3).ToList();
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
                item.Navigation_Model.Should().NotBeNull();
                item.Navigation_Model.Id.Should().Be(item.Navigation_Model_Id);
            }
        }

        //[TestMethod("ResolveODataQueryOptions | Should Works | When Navigation Model")]
        //public async Task NavigationModel_Filter_ShouldSuccess()
        //{
        //    //Arrage
        //    Random rdm = new Random();
        //    var _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
        //        .UseInMemoryDatabase(nameof(TestDbContext))
        //        .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        //        .Options;

        //    using var context = new TestDbContext(_contextOptions);

        //    context.Database.EnsureDeleted();
        //    context.Database.EnsureCreated();
        //    var navigationModel = ValueGenerator.BuildObject<Navigation_Model>().With(d => d.Name, "Navigation Name").Create();
        //    context.Add(navigationModel);

        //    var seedModels = ValueGenerator.BuildObject<Model>().With(d => d.Navigation_Model_Id, navigationModel.Id).CreateMany(3).ToList();
        //    foreach (var item in seedModels)
        //    {
        //        item.Age = rdm.Next(19, 50);
        //    }

        //    context.AddRange(seedModels);

        //    context.SaveChanges();

        //    var builder = new ODataConventionModelBuilder();
        //    builder.EntitySet<Model>(nameof(Model));
        //    var resolver = new ODataResolver(new EdmModelProvider(builder.GetEdmModel()));

        //    //Action
        //    var option = resolver.ResolveODataQueryOptions(typeof(Model), "/Model?$filter=Navigation_Model.Name eq 'Navigation Name'");
        //    var rs = option.ApplyTo(context.Set<Model>()) as IQueryable<Model>;
        //    var data = await rs.ToListAsync();
        //    //Assert
        //    data.Count.Should().Be(seedModels.Where(d => d.Age > 18 && d.Age < 40).Count());
        //    foreach (var item in data)
        //    {
        //        item.Navigation_Model.Should().NotBeNull();
        //        item.Navigation_Model.Id.Should().Be(item.Navigation_Model_Id);
        //    }
        //}
    }
}
