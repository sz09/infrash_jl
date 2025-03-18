using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Files.Shares;
using Azure.Storage.Sas;
using JobLogic.Infrastructure.Contract.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobLogic.Storage
{
    public static class StorageUtils
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

        public static Temp3DayBlob Temp3DayBlobFromExisting(this BlockBlobClient blockBlobClient)
        {
            var uri = blockBlobClient.GenerateReadOnlySasUri(3);
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

        public static Temp1DayBlob Temp1DayBlobFromExisting(this ShareFileClient client)
        {
            var uri = client.GenerateReadOnlySasUri(1);
            return Temp1DayBlob.Create(uri);
        }

        public static Temp3DayBlob Temp3DayBlobFromExisting(this ShareFileClient client)
        {
            var uri = client.GenerateReadOnlySasUri(3);
            return Temp3DayBlob.Create(uri);
        }

        public static Temp7DayBlob Temp7DayBlobFromExisting(this ShareFileClient client)
        {
            var uri = client.GenerateReadOnlySasUri(7);
            return Temp7DayBlob.Create(uri);
        }

        public static Temp30DayBlob Temp30DayBlobFromExisting(this ShareFileClient client)
        {
            var uri = client.GenerateReadOnlySasUri(30);
            return Temp30DayBlob.Create(uri);
        }

        internal static Uri GenerateReadOnlySasUri(this BlobClient blobClient, int expiredInDays)
        {
            Uri sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddDays(expiredInDays));
            return sasUri;
        }

        internal static Uri GenerateReadOnlySasUri(this ShareFileClient client, int expiredInDays)
        {
            Uri sasUri = client.GenerateSasUri(ShareFileSasPermissions.Read, DateTimeOffset.UtcNow.AddDays(expiredInDays));
            return sasUri;
        }

        internal static Uri GenerateReadOnlySasUri(this BlockBlobClient blockBlobClient, int expiredInDays)
        {
            Uri sasUri = blockBlobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddDays(expiredInDays));
            return sasUri;
        }
    }
}
