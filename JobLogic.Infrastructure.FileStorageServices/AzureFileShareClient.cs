using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System.IO;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.FileStorageServices
{
    public class AzureFileShareClient : IFileClient
    {
        private const string connectionStringTemplate = "DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2}";

        private readonly string _connectionString;
        private readonly string _shareName;

        private CloudFileShare _share;

        public bool Connected { get; private set; }

        public AzureFileShareClient(string accountName, string accountKey, string shareName, string protocol = "https")
        {
            _connectionString = string.Format(connectionStringTemplate, protocol, accountName, accountKey);
            _shareName = shareName;
        }

        public AzureFileShareClient(string connectionString, string shareName)
        {
            _connectionString = connectionString;
            _shareName = shareName;
        }

        private CloudFileDirectory GetDirectory(string directory)
        {
            var relevantDirectory = _share.GetRootDirectoryReference();

            if (directory != null)
                relevantDirectory = relevantDirectory.GetDirectoryReference(directory);

            return relevantDirectory;
        }

        private CloudFile GetFileReference(string directory, string fileName)
        {
            if (fileName == null)
                return null;

            var dir = GetDirectory(directory);
            var file = dir.GetFileReference(fileName);

            return file;
        }

        public async Task<bool> ConnectAsync()
        {
            if (Connected)
                return true;

            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            var fileClient = storageAccount.CreateCloudFileClient();
            var share = fileClient.GetShareReference(_shareName);

            Connected = await share.ExistsAsync();
            if (Connected)
                _share = share;

            return Connected;
        }

        public Task EnsureDirectoryExistsAsync(string directory)
        {
            var dir = GetDirectory(directory);
            return dir.CreateIfNotExistsAsync();
        }

        public Task<bool> ExistsAsync(string fileName)
        {
            return ExistsAsync(null, fileName);
        }

        public Task<bool> ExistsAsync(string directory, string fileName)
        {
            var file = GetFileReference(directory, fileName);
            if (file == null)
                return Task.FromResult(false);

            return file.ExistsAsync();
        }

        public async Task<Stream> ReadToStreamAsync(string directory, string fileName)
        {
            var result = new MemoryStream();
            var file = GetFileReference(directory, fileName);

            if (file == null || !await file.ExistsAsync())
                return null;

            await file.DownloadRangeToStreamAsync(result, null, null);
            result.Seek(0, SeekOrigin.Begin);

            return result;
        }

        public Task WriteStreamAsync(string directory, string fileName, Stream stream)
        {
            var file = GetFileReference(directory, fileName);
            return file.UploadFromStreamAsync(stream);
        }
    }
}