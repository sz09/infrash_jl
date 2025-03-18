using Microsoft.Extensions.Configuration;
using System;

namespace JobLogic.Infrastructure.ConfigUtilities
{
    public class ConfigUtil
    {
        IConfiguration _configurationRoot;
        private ConfigUtil() { }
        internal void Init(IConfiguration configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }
        public static ConfigUtil Instance { get; } = new ConfigUtil();

        public string GetAppSetting(string key)
        {
            if (_configurationRoot == null)
                throw new Exception($"{nameof(ConfigUtil)} is not initialized");
            return _configurationRoot[key];
        }

        public string GetConnectionString(string key)
        {
            if (_configurationRoot == null)
                throw new Exception($"{nameof(ConfigUtil)} is not initialized");
            return _configurationRoot.GetConnectionString(key);
        }
    }
}
