using System;
using System.IO;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.FileStorageServices
{
    public interface IFileShareService
    {
        Task<bool> FileExistAsync(string fileName);

        Task<Stream> GetFileAsync(string fileName);

        Task WriteStreamAsync(string fileName, Stream stream);
    }

    //WARNING - This Services used by Mobile offline, workflow service and etc. So make sure to check 

    public class FileShareService : IFileShareService
    {
        private readonly IFileClient _fileClient;
        private readonly string _directory;

        public FileShareService(IFileClient fileClient, string directory)
        {
            _fileClient = fileClient;
            _directory = directory;
        }

        public async Task<bool> FileExistAsync(string fileName)
        {
            await CheckIfFileClientIsConectedAsync();
            return await _fileClient.ExistsAsync(_directory, fileName);
        }

        public async Task<Stream> GetFileAsync(string fileName)
        {
            await CheckIfFileClientIsConectedAsync();
            return await _fileClient.ReadToStreamAsync(_directory, fileName);
        }

        public async Task WriteStreamAsync(string fileName, Stream stream)
        {
            await CheckIfFileClientIsConectedAsync();
            await _fileClient.WriteStreamAsync(_directory, fileName, stream);
        }

        private async Task CheckIfFileClientIsConectedAsync()
        {
            if (!_fileClient.Connected && !await _fileClient.ConnectAsync())
                throw new Exception("Unable to connect the file client");
        }
    }
}
