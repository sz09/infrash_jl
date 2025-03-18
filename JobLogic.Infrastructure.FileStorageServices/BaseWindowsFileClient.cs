using System.IO;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.FileStorageServices
{
    public abstract class BaseWindowsFileClient : IFileClient
    {
        public bool Connected { get; protected set; }

        public abstract Task<bool> ConnectAsync();

        public Task EnsureDirectoryExistsAsync(string directory)
        {
            var fullPath = GetFullPath(directory);
            Directory.CreateDirectory(fullPath);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string fileName)
        {
            return ExistsAsync(null, fileName);
        }

        public Task<bool> ExistsAsync(string directory, string fileName)
        {
            var fullPath = GetFullPath(directory, fileName);
            return Task.FromResult(File.Exists(fullPath));
        }

        public Task<Stream> ReadToStreamAsync(string directory, string fileName)
        {
            var fullPath = GetFullPath(directory, fileName);
            var stream = new FileStream(fullPath, FileMode.Open);

            return Task.FromResult<Stream>(stream);
        }

        public Task WriteStreamAsync(string directory, string fileName, Stream stream)
        {
            var fullPath = GetFullPath(directory, fileName);
            using (var fs = new FileStream(fullPath, FileMode.Create))
            {
                return stream.CopyToAsync(fs);
            }
        }

        protected abstract string GetFullPath(string directory);

        protected abstract string GetFullPath(string directory, string fileName);
    }
}
