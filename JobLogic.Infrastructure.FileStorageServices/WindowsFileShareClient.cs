using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.FileStorageServices
{
    public class WindowsFileShareClient : BaseWindowsFileClient, IFileClient
    {
        public string DriveLetter { get; private set; }
        public string SharePath { get; private set; }

        private readonly string _user;
        private readonly string _accessKey;

        private readonly string mountCommand;

        public WindowsFileShareClient(string driveLetter, string filesharePath, bool persistent = true)
        {
            DriveLetter = driveLetter;
            SharePath = filesharePath;

            mountCommand = $"net use {driveLetter} {filesharePath} {GetPersistentString(persistent)}";
        }

        public WindowsFileShareClient(string driveLetter, string filesharePath, string user, string accessKey, bool persistent = true)
            : this(driveLetter, filesharePath, persistent)
        {
            _user = user;
            _accessKey = accessKey;

            mountCommand = $"net use {driveLetter} {filesharePath} {GetPersistentString(persistent)} /u:{user} {accessKey}";
        }

        protected override string GetFullPath(string directory)
        {
            var fullPath = DriveLetter;

            if (directory != null)
                fullPath += "\\" + directory;
            return fullPath;
        }

        protected override string GetFullPath(string directory, string fileName)
        {
            var fullPath = GetFullPath(directory);

            fullPath += "\\" + fileName;

            return fullPath;
        }

        private string GetPersistentString(bool persistent)
        {
            if (!persistent)
                return "";
            return "/persistent:yes";
        }

        public override async Task<bool> ConnectAsync()
        {
            if (Connected || Directory.Exists(DriveLetter))
                return true;

            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"/C{mountCommand}";
            process.StartInfo = startInfo;
            process.Start();

            int sleepCount = 0;
            while (!Directory.Exists(DriveLetter) && sleepCount < 1000)
            {
                await Task.Delay(100);
                sleepCount += 100;
            }

            Connected = Directory.Exists(DriveLetter);
            return Connected;
        }
    }
}
