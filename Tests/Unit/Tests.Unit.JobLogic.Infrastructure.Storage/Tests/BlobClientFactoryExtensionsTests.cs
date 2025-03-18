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
    public class BlobClientFactoryExtensionsTests
    {
        [TestMethod]
        public async Task TestBlobClientFor_ShouldUploadSuccessfully()
        {
            //Arrange
            var httpclient = new HttpClient();
            var strm = await httpclient.GetStreamAsync("https://go.joblogic.com/favicon.ico");
            var storageClient = new GlobalPublicStorageClient( new GlobalPublicStorageClientSetting("UseDevelopmentStorage=true;", null));
            var blobContainerClient = storageClient.GetBlobContainerClient(Constants.TEST_CONTAINER);
            await blobContainerClient.CreateIfNotExistsAsync();

            //Action
            var blobClient = storageClient.GetBlobClient(Constants.TEST_CONTAINER, Guid.NewGuid().ToString());

            //Assert
            var ttt = await blobClient.UploadAsync(strm);
            ttt.Value.Should().NotBeNull();
        }
    }
}
