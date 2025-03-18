using Azure.Storage.Blobs;

namespace JobLogic.Infrastructure.Storage
{
    public interface IGeneralBlobStorageClient
    {
        BlobClient BlobClientForJLWebTemplates(string blobReference);
        BlobClient BlobClientForChasWestLetterTemplates(string blobReference);
        BlobClient BlobClientForJLWebFilelink(string blobReference);
        BlobClient BlobClientForJLWebTemp(string blobReference);
    }

    public class GeneralBlobStorageClient : IGeneralBlobStorageClient
    {
        private readonly string _cloudStorageConnection;
        public const string AzureBlobContainer = "jlweb-filelink";
        public const string DocumentTemplateBlobContainer = "jlweb-templates";
        public const string DownloadsTempBlobContainer = "jlweb-temp";
        public const string ChasWestBlobContainer = "chaswest-lettertemplates";

        public GeneralBlobStorageClient(string cloudStorageConnection)
        {
            _cloudStorageConnection = cloudStorageConnection;
        }

        public BlobClient BlobClientForJLWebFilelink(string blobReference)
        {
            var blobClient = new BlobClient(_cloudStorageConnection, AzureBlobContainer, blobReference);
            return blobClient;
        }

        public BlobClient BlobClientForChasWestLetterTemplates(string blobReference)
        {
            var blobClient = new BlobClient(_cloudStorageConnection, ChasWestBlobContainer, blobReference);
            return blobClient;
        }

        public BlobClient BlobClientForJLWebTemplates(string blobReference)
        {
            var blobClient = new BlobClient(_cloudStorageConnection, DocumentTemplateBlobContainer, blobReference);
            return blobClient;
        }

        public BlobClient BlobClientForJLWebTemp(string blobReference)
        {
            var blobClient = new BlobClient(_cloudStorageConnection, DownloadsTempBlobContainer, blobReference);
            return blobClient;
        }
    }
}