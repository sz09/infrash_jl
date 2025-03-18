using System;
using System.IO;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.FileStorageServices
{
    public sealed class FileHelper
    {
        private static readonly Lazy<FileHelper> lazy = new Lazy<FileHelper>(() => new FileHelper());

        public static FileHelper Instance { get { return lazy.Value; } }

        private FileHelper() { }

        public Task WriteContentAsync(string fileName, bool isAppend, string content)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            using (var sw = new StreamWriter(fileName, isAppend))
            {
                return sw.WriteLineAsync(content);
            }
        }

        public Task<string> ReadContentAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            using (var sr = new StreamReader(File.OpenRead(fileName)))
            {
                return sr.ReadToEndAsync();
            }
        }
    }
}
