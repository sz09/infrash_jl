using Azure.Storage.Blobs;

namespace JobLogic.Storage
{
    public interface IStorageHelper
    {
        BlobClient BlobClientFor(string containerReference, string blobReference);
    }

    public class StorageHelper : IStorageHelper
    {
        private readonly string _cloudStorageConnection;

        public StorageHelper(string cloudStorageConnection)
        {
            _cloudStorageConnection = cloudStorageConnection;
        }

        public BlobClient BlobClientFor(string containerReference, string blobReference)
        {
            var blobClient = new BlobClient(_cloudStorageConnection, containerReference, blobReference);
            return blobClient;
        }
    }
}
