using Azure.Storage.Files.Shares;
using Azure.Storage.Sas;
using JobLogic.Infrastructure.Contract.Extensions;
using System;

namespace JobLogic.Infrastructure.Storage
{
    public static class ShareFileClientExtensions
    {
        
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

        internal static Uri GenerateReadOnlySasUri(this ShareFileClient client, int expiredInDays)
        {
            Uri sasUri = client.GenerateSasUri(ShareFileSasPermissions.Read, DateTimeOffset.UtcNow.AddDays(expiredInDays));
            return sasUri;
        }
    }
}
