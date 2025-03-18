using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Storage
{
    public interface IGlobalPublicStorageClient
    {
        UploadableSASModel GenerateTemp7DayUploadableSAS(string uniqueBlobNameSuffix, bool skipCustomOriginMask = false, int linkExpireAfterInDay = 7);
        Task<BlobInfo> UploadTemp7DayBlobAsync(Stream fileContent, string uniqueBlobNameSuffix, string uniqueBlobNamePrefix = null, bool skipCustomOriginMask = false,
            int linkExpireAfterInDay = 7);
        Task<BlobInfo> UploadPublicBlobAsync(Stream fileContent, string uniqueBlobNameSuffix, bool skipCustomOriginMask = false);
    }
    public class GlobalPublicStorageClient : IGlobalPublicStorageClient
    {

        private readonly string _storageConnStr;
        private readonly string _customOrigin;

        public GlobalPublicStorageClient(GlobalPublicStorageClientSetting globalPublicStorageClientSetting)
        {
            _storageConnStr = globalPublicStorageClientSetting.StorageConnString;
            _customOrigin = globalPublicStorageClientSetting.CustomOrigin;
        }

        public const string Temp7DayContainer = "temp7day";
        public const string PublicContainer = "public";

        bool IsValidCustomOrigin()
        {
            return !string.IsNullOrEmpty(_customOrigin);
        }

        public UploadableSASModel GenerateTemp7DayUploadableSAS(string uniqueBlobNameSuffix, bool skipCustomOriginMask = false, int linkExpireAfterInDay = 7)
        {
            string blobName = StorageUtility.GenerateUniqueBlobName(uniqueBlobNameSuffix);
            var blobClient = GetBlobClient(Temp7DayContainer, blobName);

            var expiredOn = DateTime.UtcNow.AddDays(linkExpireAfterInDay);

            var uri = blobClient.GenerateSasUri(BlobSasPermissions.Create | BlobSasPermissions.Read, expiredOn);
            if (!skipCustomOriginMask && IsValidCustomOrigin())
            {
                uri = uri.ToMaskedUri(_customOrigin);
            }
            return new UploadableSASModel()
            {
                Uri = uri,
                ExpiredOn = expiredOn
            };
        }

        public async Task<BlobInfo> UploadTemp7DayBlobAsync(Stream fileContent, string uniqueBlobNameSuffix, string uniqueBlobNamePrefix = null, bool skipCustomOriginMask = false,
            int linkExpireAfterInDay = 7)
        {
            var blobName = StorageUtility.GenerateUniqueBlobName(uniqueBlobNameSuffix, uniqueBlobNamePrefix);
            var blobClient = GetBlobClient(Temp7DayContainer, blobName);
            await blobClient.UploadAsync(fileContent);

            var expiredOn = DateTime.UtcNow.AddDays(linkExpireAfterInDay);

            var uri = blobClient.GenerateSasUri(BlobSasPermissions.Read, expiredOn);

            if (!skipCustomOriginMask && IsValidCustomOrigin())
            {
                uri = uri.ToMaskedUri(_customOrigin);
            }
            return new BlobInfo(uri, blobName, Temp7DayContainer);
        }

        public async Task<BlobInfo> UploadPublicBlobAsync(Stream fileContent, string uniqueBlobNameSuffix, bool skipCustomOriginMask = false)
        {
            var blobName = StorageUtility.GenerateUniqueBlobName(uniqueBlobNameSuffix);
            var blobClient = GetBlobClient(PublicContainer, blobName);
            await blobClient.UploadAsync(fileContent);
            var uri = blobClient.Uri;
            if (!skipCustomOriginMask && IsValidCustomOrigin())
            {
                uri = uri.ToMaskedUri(_customOrigin);
            }
            return new BlobInfo(uri, blobName, PublicContainer);
        }

        public BlobClient GetBlobClient(string containerName, string blobName)
        {
            return new BlobClient(_storageConnStr, containerName, blobName);
        }

        public BlobContainerClient GetBlobContainerClient(string containerName)
        {
            return new BlobContainerClient(_storageConnStr, containerName);
        }
    }
}
