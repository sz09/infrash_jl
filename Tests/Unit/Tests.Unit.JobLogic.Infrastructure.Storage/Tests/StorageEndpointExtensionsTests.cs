using FluentAssertions;
using JobLogic.Infrastructure.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Storage
{
    [TestClass]
    public class StorageEndpointExtensionsTests
    {
        [TestMethod]
        public async Task TestGetBlobEndpointUri_ShouldWork_WhenUseLocalConnectionString()
        {
            //Arrange

            //Action
            var uri = StorageEndpointUtility.GetBlobEndpointUri("UseDevelopmentStorage=true;");

            //Assert
            uri.ToString().Should().Contain("http://").And.Contain("127.0.0.1");
        }


        [TestMethod]
        public async Task TestGetBlobEndpointUri_ShouldWork_WhenUseAzureConnectionString()
        {
            //Arrange

            //Action
            var uri = StorageEndpointUtility.GetBlobEndpointUri("DefaultEndpointsProtocol=https;AccountName=jldevpublicstorage;AccountKey=AmJq6n4RsaQ9yITgwfxvIyQPoLnAxQkbfc2e/MdQpwosxFraLIzhZgv71itmRV0mtCq4GehT0nMH8X2/eWHNyQ==;EndpointSuffix=core.windows.net");

            //Assert
            uri.ToString().Should().Contain("https://").And.Contain("jldevpublicstorage.blob.core.windows.net");
        }


        [TestMethod]
        public async Task TestGetShareFileEndpointUri_ShouldWork_WhenUseAzureConnectionString()
        {
            //Arrange

            //Action
            var uri = StorageEndpointUtility.GetFileEndpointUri("DefaultEndpointsProtocol=https;AccountName=jldevpublicstorage;AccountKey=AmJq6n4RsaQ9yITgwfxvIyQPoLnAxQkbfc2e/MdQpwosxFraLIzhZgv71itmRV0mtCq4GehT0nMH8X2/eWHNyQ==;EndpointSuffix=core.windows.net");

            //Assert
            uri.ToString().Should().Contain("https://").And.Contain("jldevpublicstorage.file.core.windows.net");
        }
    }
}
