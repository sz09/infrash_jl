using FluentAssertions;
using JobLogic.Infrastructure.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Storage
{
    [TestClass]
    public class GlobalPublicStorageClientTests
    {
        [TestMethod]
        public async Task TestUploadPublicBlobAsync_ShouldSuccess()
        {
            //Arrange
            var httpclient = new HttpClient();
            var strm = await httpclient.GetStreamAsync("https://go.joblogic.com/favicon.ico");
            var storageClient = new GlobalPublicStorageClient(new GlobalPublicStorageClientSetting("UseDevelopmentStorage=true;", null));
            var blobContainerClient = storageClient.GetBlobContainerClient(GlobalPublicStorageClient.PublicContainer);
            await blobContainerClient.CreateIfNotExistsAsync();

            //Action
            var info = await storageClient.UploadPublicBlobAsync(strm, Guid.NewGuid().ToString());

            //Assert
            info.Should().NotBeNull();
        }

        [TestMethod]
        public async Task TestGenerateTemp7DayUploadableSAS_ShouldSuccess()
        {
            //Arrange
            var storageClient = new GlobalPublicStorageClient(new GlobalPublicStorageClientSetting("UseDevelopmentStorage=true;", null));

            //Action
            var info = storageClient.GenerateTemp7DayUploadableSAS(Guid.NewGuid().ToString());

            //Assert
            info.Should().NotBeNull();
        }
    }
}
