using System;

namespace JobLogic.Infrastructure.FileStorageServices
{
    public class FileStorageDownloadResponse
    {
        public byte[] FileBinary { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string AzureBlobReference { get; set; }
        public string PublicUrl { get; set; }
    }

    public class FileStorageUploadResponse
    {
        public Guid GuidId { get; set; }
        public string FileName { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
    }
}