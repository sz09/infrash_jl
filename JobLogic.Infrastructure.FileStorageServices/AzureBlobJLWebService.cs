namespace JobLogic.Infrastructure.FileStorageServices
{
    public static class AzureBlobService_Response
    {
        public const string file_is_empty = "Cannot upload empty file";
        public const string fileType_is_empty = "File type is empty";
        public const string failed_to_upload = "Failed to upload file";
        public const string failed_to_retrieve = "Failed to retrieve file";
        public const string file_not_found = "File not found";
        public const string file_type_not_allow_upload = "File type not allowed";
    }

    public class AzureBlobJLWebService
    {
        public const string ContainerReference = "jlweb-filelink";
    }
}
