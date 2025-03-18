using JobLogic.Infrastructure.Microservice.Client;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Server.Host
{
    public class ServiceBusDispatcherSetting
    {
        public ServiceBusDispatcherSetting(IConfigurationRefresherProvider configurationRefresherProvider, ILogger<ServiceBusDispatcherSetting> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ConfigurationRefresherProvider = configurationRefresherProvider ?? throw new ArgumentNullException(nameof(configurationRefresherProvider));

        }

        public IConfigurationRefresherProvider ConfigurationRefresherProvider { get; }
        public ILogger<ServiceBusDispatcherSetting> Logger { get; }
    }
    public static class ServiceBusDispatcherExtensions
    {
        public static async Task DispatchAsync(this IDispatcher dispatcher, ServiceBusDispatcherSetting serviceBusDispatcherSetting, string serializedMicroservicePayload)
        {
            try
            {
                var payload = JsonConvert.DeserializeObject<MicroservicePayload>(serializedMicroservicePayload);

                try
                {
                    Activity.Current.AddTag("MessageSignature", payload.MessageSignature);
                }
                catch(Exception ex)
                {
                    serviceBusDispatcherSetting.Logger.LogError(ex, "Failed to log MessageSignature");
                }

                await serviceBusDispatcherSetting.ConfigurationRefresherProvider.TryRefreshWithFirstRefresherAsync();
                var result = await dispatcher.DispatchAsync(payload);
                if (result.State == InvocationResult.ResultState.Cacncelled)
                {
                    serviceBusDispatcherSetting.Logger.LogDispatchCancelled(result.CancelMessage, serializedMicroservicePayload);
                }
            }
            catch (Exception e)
            {
                serviceBusDispatcherSetting.Logger.LogDispatchException(e, serializedMicroservicePayload);
                throw;
            }
        }
    }
}
