using System;
using System.IO;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.FileStorageServices
{
    public class WindowsFileClient : BaseWindowsFileClient, IFileClient
    {
        private readonly Func<string, string> _pathResolver;

        public WindowsFileClient() { }

        public WindowsFileClient(Func<string, string> pathResolver)
        {
            _pathResolver = pathResolver;
        }

        public override Task<bool> ConnectAsync()
        {
            return Task.FromResult(true);
        }

        protected override string GetFullPath(string directory)
        {
            if (_pathResolver != null)
                return _pathResolver(directory);
            return directory;
        }

        protected override string GetFullPath(string directory, string fileName)
        {
            var fullDirectoryPath = GetFullPath(directory);
            return Path.Combine(fullDirectoryPath, fileName);
        }
    }
}
