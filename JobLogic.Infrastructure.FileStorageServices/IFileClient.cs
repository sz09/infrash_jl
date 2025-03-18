using System.IO;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.FileStorageServices
{
    public interface IFileClient
    {
        bool Connected { get; }

        Task<bool> ConnectAsync();
        Task EnsureDirectoryExistsAsync(string directory);
        Task<bool> ExistsAsync(string fileName);
        Task<bool> ExistsAsync(string directory, string fileName);
        Task<Stream> ReadToStreamAsync(string directory, string fileName);        
        Task WriteStreamAsync(string directory, string fileName, Stream stream);
    }
}