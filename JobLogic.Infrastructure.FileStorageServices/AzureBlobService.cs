using JobLogic.Infrastructure.ImageHelper;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.FileStorageServices
{
    public interface IAzureBlobService
    {
        Task UploadAsync(Stream inputStream, string containerReference, string blobReference, CancellationToken cancellationToken = default);
        /// <summary>
        /// Generate public URL for Blob Container
        /// </summary>
        /// <param name="containerReference">Blob container name</param>
        /// <param name="blobReference">Filename</param>
        /// <param name="ttl">Url expiration time</param>
        /// <param name="permissions">SharedAccessBlobPermissions : default is Read</param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// Error : return string.Empty
        /// Success : return Url
        /// </returns>
        Task<string> GeneratePublicUrlAsync(string containerReference, string blobReference, TimeSpan ttl, SharedAccessBlobPermissions permissions = SharedAccessBlobPermissions.Read, CancellationToken cancellationToken = default);       

        Task<byte[]> DownloadAsync(string containerReference, string blobReference, CancellationToken cancellationToken = default);

        Task<byte[]> DownloadAsync(string containerReference, string blobReference, bool getThumbnail, CancellationToken cancellationToken = default);

        Task<byte[]> DownloadAndDeleteAsync(string containerReference, string blobReference, CancellationToken cancellationToken = default);

        Task DeleteAsync(string containerReference, string blobReference, CancellationToken cancellationToken = default);

        Task DeleteAsync(string containerReference, IEnumerable<string> blobReferences, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(string containerReference, string blobReference, CancellationToken cancellationToken = default);
    }

    public class AzureBlobService : IAzureBlobService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        static readonly Random _random = new Random();

        public AzureBlobService(string cloudStorageConnection)
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnection);
        }

        #region Utilities
        private CloudBlobContainer GetCloudBlobContainer(string containerReference)
        {
            containerReference = GetContainerReference(containerReference);
            var blobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerReference);

            return container;
        }

        private CloudBlockBlob GetBlockBlobReference(string containerReference, string blobReference)
        {
            var blobContainer = GetCloudBlobContainer(containerReference);
            return GetBlockBlobReference(blobContainer, blobReference);
        }

        private static string GetContainerReference(string containerReference)
        {
            if (string.IsNullOrWhiteSpace(containerReference))
            {
                containerReference = AzureBlobJLWebService.ContainerReference;
            }

            return containerReference;
        }

        private static string CreateThumbnailBlobReference(string blobReference)
        {
            var extenstion = blobReference.GetExtension();
            return blobReference.RemoveSuffix(extenstion) + "_thumbnail" + extenstion;
        }

        private static CloudBlockBlob GetBlockBlobReference(CloudBlobContainer blobContainer, string blobReference)
        {
            if (string.IsNullOrWhiteSpace(blobReference)) return null;

            return blobContainer.GetBlockBlobReference(blobReference);
        }

        private static async Task<byte[]> DownloadAsync(CloudBlockBlob blockBlobReference)
        {
            await blockBlobReference.FetchAttributesAsync();

            var fileBinary = new byte[blockBlobReference.Properties.Length];
            await blockBlobReference.DownloadToByteArrayAsync(fileBinary, 0);

            return fileBinary;
        }

        private static Task<byte[]> DownloadAsync(CloudBlobContainer blobContainer, string blobReference)
        {
            var blockBlobReference = GetBlockBlobReference(blobContainer, blobReference);
            if (blockBlobReference == null)
                return null;

            return DownloadAsync(blockBlobReference);
        }

        private static async Task<byte[]> UploadThumbnailAsync(CloudBlobContainer blobContainer, string blobReference, Stream inputStream)
        {
            inputStream.Position = 0;
            if (!string.IsNullOrWhiteSpace(blobReference) && inputStream.IsImage())
            {
                inputStream.Position = 0;
                using (var inMemoryImage = Image.FromStream(inputStream))
                {
                    using (var imgStream = new MemoryStream())
                    {
                        inMemoryImage.GetImageSize(out int height, out int width);
                        using (var bitmap = inMemoryImage.Resize(width, height))
                        {
                            bitmap.Save(imgStream, ImageFormat.Jpeg);
                        }

                        imgStream.Position = 0;
                        var thumbnailBlockBlobReference = GetBlockBlobReference(blobContainer, CreateThumbnailBlobReference(blobReference));
                        await thumbnailBlockBlobReference.UploadFromStreamAsync(imgStream);

                        imgStream.Position = 0;
                        return imgStream.ToArray();
                    }
                }
            }

            return null;
        }

        #endregion

        #region Async methods
        public async Task UploadAsync(Stream inputStream, string containerReference, string blobReference, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var blobContainer = GetCloudBlobContainer(containerReference);
            var blockBlobReference = GetBlockBlobReference(blobContainer, blobReference);

            await blockBlobReference.UploadFromStreamAsync(inputStream);
            await UploadThumbnailAsync(blobContainer, blobReference, inputStream);
        }

        public async Task<string> GeneratePublicUrlAsync(string containerReference, string blobReference, TimeSpan ttl, SharedAccessBlobPermissions permissions = SharedAccessBlobPermissions.Read, CancellationToken cancellationToken = default) {
            if (string.IsNullOrWhiteSpace(containerReference) || string.IsNullOrWhiteSpace(blobReference))
                return string.Empty;
            var blobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerReference);
            var containerExisted = await blobContainer.ExistsAsync();
            if (!containerExisted)
                return string.Empty;
            var blob = blobContainer.GetBlockBlobReference(blobReference);
            var blobExisted = await blob.ExistsAsync();
            if (!blobExisted)
                return string.Empty;
            var shareAccessSignature = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.Add(ttl),
                Permissions = permissions
            });
            return blob.Uri + shareAccessSignature;
        }

        public Task<byte[]> DownloadAsync(string containerReference, string blobReference, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return DownloadAsync(containerReference, blobReference, false);
        }

        public async Task<byte[]> DownloadAsync(string containerReference, string blobReference, bool getThumbnail, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var blobContainer = GetCloudBlobContainer(containerReference);
            if (getThumbnail && !string.IsNullOrWhiteSpace(blobReference) && blobReference.IsImage())
            {
                var thumbnailBlockBlobReference = GetBlockBlobReference(blobContainer, CreateThumbnailBlobReference(blobReference));
                if (thumbnailBlockBlobReference == null || !await thumbnailBlockBlobReference.ExistsAsync())
                {
                    var blob = await DownloadAsync(blobContainer, blobReference);
                    using (var original = new MemoryStream(blob))
                    {
                        return await UploadThumbnailAsync(blobContainer, blobReference, original);
                    }
                }
                else
                {
                    return await DownloadAsync(thumbnailBlockBlobReference);
                }
            }

            return await DownloadAsync(blobContainer, blobReference);
        }

        public async Task<byte[]> DownloadAndDeleteAsync(string containerReference, string blobReference, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var document = await DownloadAsync(containerReference, blobReference);
            if (document != null)
            {
                await DeleteAsync(containerReference, blobReference);
            }

            return document;
        }

        public Task<bool> ExistsAsync(string containerReference, string blobReference, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var blockBlobReference = GetBlockBlobReference(containerReference, blobReference);
            return blockBlobReference.ExistsAsync();
        }

        public Task DeleteAsync(string containerReference, IEnumerable<string> blobReferences, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var blobContainer = GetCloudBlobContainer(containerReference);
            var tasks = blobReferences.Select(i => GetBlockBlobReference(blobContainer, i)).Select(i => i.DeleteIfExistsAsync());
            return Task.WhenAll(tasks);
        }

        public Task DeleteAsync(string containerReference, string blobReference, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return DeleteAsync(containerReference, new string[] { blobReference });
        }    

        #endregion
    }

    static class Extensions
    {
        public static string GetExtension(this string fileName, bool withDot = true)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var fileExtPos = fileName.LastIndexOf(".");
                if (fileExtPos >= 0)
                {
                    return fileName.Substring(fileExtPos + (withDot ? 0 : 1));
                }
            }

            return string.Empty;
        }

        public static string RemoveSuffix(this string value, string suffix)
        {
            var result = "";
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.TrimEnd();
                result = (!string.IsNullOrWhiteSpace(suffix) && value.EndsWith(suffix)) ? value.Remove(value.LastIndexOf(suffix)) : value;
            }
            return result;
        }
    }
}
