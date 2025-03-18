#if NETCORE
using Microsoft.Extensions.Configuration;
using System;
#else
using System.Configuration;
#endif

namespace JobLogic.Infrastructure.ConfigurationHelper
{
    public abstract class ConfigurationHelper : IConfigurationHelper
    {
        private static ConfigurationHelper _instance;
        public static ConfigurationHelper Instance
        {
            get
            {
                if (_instance == null)
                {
#if NETCORE
                    _instance = new NetCoreConfigurationHelper();
#else
                    _instance = new NetFxConfigurationHelper();
#endif
                }
                return _instance;
            }
        }

        public abstract string GetAppSetting(string key);

        public abstract string GetConnectionString(string key);
    }

#if NETCORE
    internal class NetCoreConfigurationHelper : ConfigurationHelper
    {
        private readonly IConfigurationRoot configurationBuilder;

        public NetCoreConfigurationHelper()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(environment))
            {
                environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
            }

            configurationBuilder = new ConfigurationBuilder()
              .SetBasePath(Environment.CurrentDirectory)
              .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build();
        }

        public override string GetAppSetting(string key)
        {
            return configurationBuilder[key];
        }

        public override string GetConnectionString(string key)
        {
            return configurationBuilder.GetConnectionString(key);
        }
    }
#else
    internal class NetFxConfigurationHelper : ConfigurationHelper
    {
        public override string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public override string GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key]?.ConnectionString;
        }
    }
#endif
}
