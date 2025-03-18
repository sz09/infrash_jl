using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System;

namespace JobLogic.Infrastructure.ConfigUtilities
{
    public static class ConfigUtilInitializer
    {
        public static void Init(IConfiguration configurationRoot)
        {
            ConfigUtil.Instance.Init(configurationRoot);
        }

        public static void AddAACForWebAppNetCore(this IConfigurationBuilder configBuilder, string appPrefixWithSlash = null)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            const string SharePrefix = "Share/";
            var azureAppConfigurationConnStr = Environment.GetEnvironmentVariable("JLAzureAppConfigurationConnStr");
            const string SentinelKey = "ShareSentinel";

            configBuilder.AddAzureAppConfiguration(options =>
            {
                options
                    .Connect(azureAppConfigurationConnStr)
                    .Select($"{SharePrefix}*").TrimKeyPrefix(SharePrefix)
                    .Select($"{SharePrefix}*", envName).TrimKeyPrefix(SharePrefix);

                if (!string.IsNullOrEmpty(appPrefixWithSlash))
                {
                    options.Select($"{appPrefixWithSlash}*").TrimKeyPrefix(appPrefixWithSlash)
                    .Select($"{appPrefixWithSlash}*", envName).TrimKeyPrefix(appPrefixWithSlash);
                }

                options
                    .UseFeatureFlags(x => x.CacheExpirationInterval = TimeSpan.FromMinutes(3))
                    .UseFeatureFlags(m =>
                    {
                        m.CacheExpirationInterval = TimeSpan.FromMinutes(3);
                        m.Label = envName;
                    });

                options.ConfigureRefresh(refresh =>
                {
                    refresh.Register(SentinelKey, refreshAll: true).SetCacheExpiration(TimeSpan.FromMinutes(3));
                });

                options.ConfigureKeyVault(option =>
                {
                    option.SetCredential(new DefaultAzureCredential());
                });
            });

            configBuilder
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true);

            configBuilder.AddEnvironmentVariables();
        }

        public static IConfiguration InitForConsoleApp(string appPrefixWithSlash = null)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configRoot = configBuilder.Build();

            var envName = configRoot["JL_ENVIRONMENT"];
            const string SharePrefix = "Share/";
            var azureAppConfigurationConnStr = configRoot["JLAzureAppConfigurationConnStr"];

            configBuilder.AddAzureAppConfiguration(options =>
            {
                options
                    .Connect(azureAppConfigurationConnStr)
                    .Select($"{SharePrefix}*").TrimKeyPrefix(SharePrefix)
                    .Select($"{SharePrefix}*", envName).TrimKeyPrefix(SharePrefix);
                if (!string.IsNullOrEmpty(appPrefixWithSlash))
                {
                    options.Select($"{appPrefixWithSlash}*").TrimKeyPrefix(appPrefixWithSlash)
                    .Select($"{appPrefixWithSlash}*", envName).TrimKeyPrefix(appPrefixWithSlash);
                }

                options.ConfigureKeyVault(option =>
                {
                    option.SetCredential(new DefaultAzureCredential());
                });
            });

            configBuilder
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            configBuilder.AddEnvironmentVariables();

            configRoot = configBuilder.Build();

            Init(configRoot);

            return configRoot;
        }
    }
}
