using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using JobLogic.Infrastructure.Contract.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Storage
{
    public interface ITempStorageClient
    {
        Task<Temp1DayBlob> CreateTemp1DaysBlobAsync(Stream inputStream, string fileHintPrefix = null, string fileHintSuffix = null);
        Task<Temp3DayBlob> CreateTemp3DaysBlobAsync(Stream inputStream, string fileHintPrefix = null, string fileHintSuffix = null);
        Task<Temp7DayBlob> CreateTemp7DaysBlobAsync(Stream inputStream, string fileHintPrefix = null, string fileHintSuffix = null);
        Task<Temp30DayBlob> CreateTemp30DaysBlobAsync(Stream inputStream, string fileHintPrefix = null, string fileHintSuffix = null);
    }

    public class TempStorageClient : ITempStorageClient
    {
        static readonly Random _random = new Random();
        private readonly string _storageConnStr;

        public TempStorageClient(string storageConnStr)
        {
            _storageConnStr = storageConnStr;
        }

        public async Task<Temp1DayBlob> CreateTemp1DaysBlobAsync(Stream inputStream, string fileHintPrefix = null, string fileHintSuffix = null)
        {
            var uri = await CreateTempBlobAsync(inputStream, fileHintPrefix, fileHintSuffix, TempStorageBlobConst.Temp1DayContainer, TempStorageBlobConst.Temp1DayExpire);
            return Temp1DayBlob.Create(uri);
        }

        public async Task<Temp3DayBlob> CreateTemp3DaysBlobAsync(Stream inputStream, string fileHintPrefix = null, string fileHintSuffix = null)
        {
            var uri = await CreateTempBlobAsync(inputStream, fileHintPrefix, fileHintSuffix, TempStorageBlobConst.Temp3DayContainer, TempStorageBlobConst.Temp3DayExpire);
            return Temp3DayBlob.Create(uri);
        }

        public async Task<Temp7DayBlob> CreateTemp7DaysBlobAsync(Stream inputStream, string fileHintPrefix = null, string fileHintSuffix = null)
        {
            var uri = await CreateTempBlobAsync(inputStream, fileHintPrefix, fileHintSuffix, TempStorageBlobConst.Temp7DayContainer, TempStorageBlobConst.Temp7DayExpire);
            return Temp7DayBlob.Create(uri);
        }

        public async Task<Temp30DayBlob> CreateTemp30DaysBlobAsync(Stream inputStream, string fileHintPrefix = null, string fileHintSuffix = null)
        {
            var uri = await CreateTempBlobAsync(inputStream, fileHintPrefix, fileHintSuffix, TempStorageBlobConst.Temp30DayContainer, TempStorageBlobConst.Temp30DayExpire);
            return Temp30DayBlob.Create(uri);
        }

        #region private method

        private async Task<Uri> CreateTempBlobAsync(Stream inputStream, string fileHintPrefix, string fileHintSuffix, string containerName, int expiredInDays)
        {
            fileHintPrefix = string.IsNullOrEmpty(fileHintPrefix) ? "NoHint" : fileHintPrefix;
            fileHintSuffix = string.IsNullOrEmpty(fileHintSuffix) ? string.Empty : "/" + fileHintSuffix;
            string blobName = $"{fileHintPrefix}_{DateTime.UtcNow:HHmmssffff}_{Guid.NewGuid()}_{_random.Next(int.MaxValue)}{fileHintSuffix}";

            var blobClient = GetBlobClient(containerName, blobName);
            await blobClient.UploadAsync(inputStream);

            var expiredOn = DateTime.UtcNow.AddDays(expiredInDays);
            return blobClient.GenerateSasUri(BlobSasPermissions.Read, expiredOn);
        }

        private BlobClient GetBlobClient(string containerName, string blobName)
        {
            return new BlobClient(_storageConnStr, containerName, blobName);
        }

        #endregion

    }
}
