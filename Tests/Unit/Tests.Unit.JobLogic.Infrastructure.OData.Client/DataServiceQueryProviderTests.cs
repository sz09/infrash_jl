using FluentAssertions;
using Microsoft.OData.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Tests.Unit.JobLogic.Infrastructure.OData.Client
{
    [TestClass]
    public class DataServiceQueryProviderTests
    {
        const string ServiceUri = "http://fakeodataendpoint";
        public DataServiceQueryProviderTests()
        {
        }
        [TestMethod]
        public void TranslatesEnumerableContainsWithSpecialCharactersToInOperator()
        {
            var clause = new[] { "a & b" };
            var ctx = new DataServiceContext(new Uri(ServiceUri));
            var odataQuery = ctx.CreateQuery<Model>(typeof(Model).Name).Where(x => clause.Contains(x.Content)) as DataServiceRequest;
            odataQuery.RequestUri.Should().NotBeNull();
            //Microsoft.OData.Client" Version="7.8.3"
            Assert.AreNotEqual(odataQuery.RequestUri.ToString(), $"{ServiceUri}/Model?$filter=Content in ('a & b')");
            //Microsoft.OData.Client" Version="7.12.1"
            Assert.AreEqual(odataQuery.RequestUri.ToString(), $"{ServiceUri}/Model?$filter=Content in ('a %26 b')");
        }
    }
}
