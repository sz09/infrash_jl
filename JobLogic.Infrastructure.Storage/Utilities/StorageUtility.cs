using Azure.Storage.Blobs;
using JobLogic.Infrastructure.Contract.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Storage
{

    public static class StorageUtility 
    {
        static readonly Random _random = new Random();

        public static string GenerateUniqueBlobName(string fileHintSuffix, string fileHintPrefix = null)
        {
            fileHintPrefix = string.IsNullOrEmpty(fileHintPrefix) ? "" : fileHintPrefix + "_";
            fileHintSuffix = string.IsNullOrEmpty(fileHintSuffix) ? "undefiedName" : fileHintSuffix;
            string blobName = $"{fileHintPrefix}{DateTime.UtcNow:yyMMdd}/{DateTime.UtcNow:HHmmssffff}_{Guid.NewGuid()}_{_random.Next(int.MaxValue)}/{fileHintSuffix}";
            return blobName;
        }

        public static Uri ToMaskedUri(this Uri uri, string customOrigin)
        {
            customOrigin = customOrigin.TrimEnd('/');
            var customUriString = customOrigin + uri.PathAndQuery;
            return new Uri(customUriString);
        }
    }
}
