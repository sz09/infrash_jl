using System;

namespace JobLogic.Infrastructure.Storage
{
    public class UploadableSASModel
    {
        public Uri Uri { get; set; }
        public DateTime ExpiredOn { get; set; }
    }

    public class BlobInfo
    {
        public BlobInfo(Uri blobUri, string blobName, string blobContainer)
        {
            BlobUri = blobUri;
            BlobName = blobName;
            BlobContainer = blobContainer;
        }

        public Uri BlobUri { get; set; }
        public string BlobName { get; set; }
        public string BlobContainer { get; set; }
    }
}
