using Azure.Storage.Blobs;
using FluentAssertions;
using JobLogic.Infrastructure.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Storage
{
    [TestClass]
    public class TempStorageClientTests
    {
        [TestMethod]
        public async Task TestCreateTemp1DaysBlobAsync_ShouldSuccess()
        {
            //Arrange
            var httpclient = new HttpClient();
            var strm = await httpclient.GetStreamAsync("https://go.joblogic.com/favicon.ico");
            var connectionString = "UseDevelopmentStorage=true;";
            var tempStorageClient = new TempStorageClient(connectionString);
            var blobContainerClient = new BlobContainerClient(connectionString, "temp1day");
            await blobContainerClient.CreateIfNotExistsAsync();

            //Action
            var info = await tempStorageClient.CreateTemp1DaysBlobAsync(strm);

            //Assert
            info.Should().NotBeNull();
            info.Link.Should().NotBeNull();
            info.SasBlobUrl.Should().NotBeNull();
        }

        [TestMethod]
        public async Task TestCreateTemp3DaysBlobAsync_ShouldSuccess()
        {
            //Arrange
            var httpclient = new HttpClient();
            var strm = await httpclient.GetStreamAsync("https://go.joblogic.com/favicon.ico");
            var connectionString = "UseDevelopmentStorage=true;";
            var tempStorageClient = new TempStorageClient(connectionString);
            var blobContainerClient = new BlobContainerClient(connectionString, "temp3day");
            await blobContainerClient.CreateIfNotExistsAsync();

            //Action
            var info = await tempStorageClient.CreateTemp3DaysBlobAsync(strm);

            //Assert
            info.Should().NotBeNull();
            info.Link.Should().NotBeNull();
            info.SasBlobUrl.Should().NotBeNull();
        }

        [TestMethod]
        public async Task TestCreateTemp7DaysBlobAsync_ShouldSuccess()
        {
            //Arrange
            var httpclient = new HttpClient();
            var strm = await httpclient.GetStreamAsync("https://go.joblogic.com/favicon.ico");
            var connectionString = "UseDevelopmentStorage=true;";
            var tempStorageClient = new TempStorageClient(connectionString);
            var blobContainerClient = new BlobContainerClient(connectionString, "temp7day");
            await blobContainerClient.CreateIfNotExistsAsync();

            //Action
            var info = await tempStorageClient.CreateTemp7DaysBlobAsync(strm);

            //Assert
            info.Should().NotBeNull();
            info.Link.Should().NotBeNull();
            info.SasBlobUrl.Should().NotBeNull();
        }

        [TestMethod]
        public async Task TestCreateTemp30DaysBlobAsync_ShouldSuccess()
        {
            //Arrange
            var httpclient = new HttpClient();
            var strm = await httpclient.GetStreamAsync("https://go.joblogic.com/favicon.ico");
            var connectionString = "UseDevelopmentStorage=true;";
            var tempStorageClient = new TempStorageClient(connectionString);
            var blobContainerClient = new BlobContainerClient(connectionString, "temp30day");
            await blobContainerClient.CreateIfNotExistsAsync();

            //Action
            var info = await tempStorageClient.CreateTemp30DaysBlobAsync(strm);

            //Assert
            info.Should().NotBeNull();
            info.Link.Should().NotBeNull();
            info.SasBlobUrl.Should().NotBeNull();
        }
    }
}
