using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;
using System;
using System.Collections.Concurrent;

namespace JobLogic.Infrastructure.Storage
{
    public static class StorageEndpointUtility
    {
        private static ConcurrentDictionary<string, Uri> _blobEndpointCacheDict = new ConcurrentDictionary<string, Uri>();
        private static ConcurrentDictionary<string, Uri> _shareFileEndpointCacheDict = new ConcurrentDictionary<string, Uri>();

        public static Uri GetBlobEndpointUri(string storageConnStr)
        {
            return _blobEndpointCacheDict.GetOrAdd(storageConnStr, x =>
            {
                var serviceClient = new BlobServiceClient(x);
                return serviceClient.Uri;
            });
        }

        public static Uri GetFileEndpointUri(string storageConnStr)
        {
            return _shareFileEndpointCacheDict.GetOrAdd(storageConnStr, x =>
            {
                var serviceClient = new ShareServiceClient(x);
                return serviceClient.Uri;
            });
        }
    }
}
