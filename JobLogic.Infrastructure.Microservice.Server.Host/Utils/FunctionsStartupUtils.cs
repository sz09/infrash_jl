using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

namespace JobLogic.Infrastructure.Microservice.Server.Host
{
    public static class FunctionsStartupUtils
    {
        public static void BuildConfiguration(this IFunctionsConfigurationBuilder builder,
            string appPrefixWithSlash, string sharePrefixWithSlash = "Share/",
            string cacheSentinelKey = "ShareSentinel", int cacheExpirationInSeconds = 180,
            string JL_ENVIRONMENT_EnvVariableName = "JL_ENVIRONMENT",
            string JLAzureAppConfigurationConnStr_EnvVariableName = "JLAzureAppConfigurationConnStr")
        {
            var envName = Environment.GetEnvironmentVariable(JL_ENVIRONMENT_EnvVariableName);
            var azureAppConfigurationConnStr = Environment.GetEnvironmentVariable(JLAzureAppConfigurationConnStr_EnvVariableName);

            builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
            {
                options
                    .Connect(azureAppConfigurationConnStr)
                    .Select($"{sharePrefixWithSlash}*").TrimKeyPrefix(sharePrefixWithSlash)
                    .Select($"{sharePrefixWithSlash}*", envName).TrimKeyPrefix(sharePrefixWithSlash)
                    .Select($"{appPrefixWithSlash}*").TrimKeyPrefix(appPrefixWithSlash)
                    .Select($"{appPrefixWithSlash}*", envName).TrimKeyPrefix(appPrefixWithSlash)
                    .ConfigureRefresh(refresh =>
                    {
                        refresh.Register(cacheSentinelKey, refreshAll: true).SetCacheExpiration(TimeSpan.FromSeconds(cacheExpirationInSeconds));
                    });

                options.ConfigureKeyVault(option =>
                {
                    option.SetCredential(new DefaultAzureCredential());
                });
            });

            builder.ConfigurationBuilder.AddEnvironmentVariables();
        }
    }
}
