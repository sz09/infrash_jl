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
    public class BlobContainerClientFactoryExtensionsTests
    {
        
        [TestMethod]
        public async Task TestBlobContainerClientFor_ShouldDeleteSuccessfully()
        {
            //Arrange
            var storageClient = new GlobalPublicStorageClient(new GlobalPublicStorageClientSetting("UseDevelopmentStorage=true;", null));
            var blobContainerClient = storageClient.GetBlobContainerClient(Constants.TEST_CONTAINER);
            await blobContainerClient.CreateIfNotExistsAsync();
            var doExist = await blobContainerClient.ExistsAsync();
            doExist.Value.Should().BeTrue();
            //Action
            await blobContainerClient.DeleteAsync();

            //Assert
            doExist = await blobContainerClient.ExistsAsync();
            doExist.Value.Should().BeFalse();
        }
    }
}
