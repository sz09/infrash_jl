using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using JobLogic.Infrastructure.Contract.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobLogic.Infrastructure.Storage
{
    public static class BlobClientExtensions
    {
        public static Temp1DayBlob Temp1DayBlobFromExisting(this BlobClient blobClient)
        {
            var uri = blobClient.GenerateReadOnlySasUri(1);
            return Temp1DayBlob.Create(uri);
        }

        public static Temp3DayBlob Temp3DayBlobFromExisting(this BlobClient blobClient)
        {
            var uri = blobClient.GenerateReadOnlySasUri(3);
            return Temp3DayBlob.Create(uri);
        }

        public static Temp7DayBlob Temp7DayBlobFromExisting(this BlobClient blobClient)
        {
            var uri = blobClient.GenerateReadOnlySasUri(7);
            return Temp7DayBlob.Create(uri);
        }

        public static Temp30DayBlob Temp30DayBlobFromExisting(this BlobClient blobClient)
        {
            var uri = blobClient.GenerateReadOnlySasUri(30);
            return Temp30DayBlob.Create(uri);
        }

        internal static Uri GenerateReadOnlySasUri(this BlobClient blobClient, int expiredInDays)
        {
            Uri sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddDays(expiredInDays));
            return sasUri;
        }
    }
}
