using JobLogic.Infrastructure.Microservice.Client;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RedLockNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Server.Host
{
    public class TimerDispatcherSetting
    {
        public TimerDispatcherSetting(IConfigurationRefresherProvider configurationRefresherProvider, ILogger<TimerDispatcherSetting> logger, 
            IDistributedLockFactory distributedLockFactory, int defaultSafetyLockExpTimeInSecond = 30)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ConfigurationRefresherProvider = configurationRefresherProvider ?? throw new ArgumentNullException(nameof(configurationRefresherProvider));
            DistributedLockFactory = distributedLockFactory ?? throw new ArgumentNullException(nameof(distributedLockFactory));

            DefaultSafetyLockExpTimeInSecond = defaultSafetyLockExpTimeInSecond;
        }

        public IConfigurationRefresherProvider ConfigurationRefresherProvider { get; }
        public ILogger<TimerDispatcherSetting> Logger { get; }
        public IDistributedLockFactory DistributedLockFactory { get; }
        public int DefaultSafetyLockExpTimeInSecond { get; }
    }
    public static class TimerDispatcherExtensions
    {
        public static Task DispatchAsync(this IDispatcher dispatcher, TimerDispatcherSetting timerDispatcherSetting, TenancyMsg req, Guid tenantId, IReadOnlyDictionary<string, string> creationData = null)
        {
            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(tenantId, creationData), req);
            return dispatcher.DispatchAsync(timerDispatcherSetting, payload);
        }

        public static Task DispatchAsync(this IDispatcher dispatcher, TimerDispatcherSetting timerDispatcherSetting, TenantlessMsg req, IReadOnlyDictionary<string, string> creationData = null)
        {
            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(creationData), req);
            return dispatcher.DispatchAsync(timerDispatcherSetting, payload);
        }
        private static async Task DispatchAsync(this IDispatcher dispatcher, TimerDispatcherSetting timerDispatcherSetting, MicroservicePayload payload)
        {
            var serializedMicroservicePayload = JsonConvert.SerializeObject(payload);
            var key = "SafetyLockTimer:" + payload.MessageSignature;
            using (var redlock = await timerDispatcherSetting.DistributedLockFactory.CreateLockAsync(key, TimeSpan.FromSeconds(timerDispatcherSetting.DefaultSafetyLockExpTimeInSecond)))
            {
                if (redlock.IsAcquired)
                {
                    try
                    {
                        await timerDispatcherSetting.ConfigurationRefresherProvider.TryRefreshWithFirstRefresherAsync();
                        var result = await dispatcher.DispatchAsync(payload);
                        if (result.State == InvocationResult.ResultState.Cacncelled)
                        {
                            timerDispatcherSetting.Logger.LogDispatchCancelled(result.CancelMessage, serializedMicroservicePayload);
                        }
                    }
                    catch (Exception e)
                    {
                        timerDispatcherSetting.Logger.LogDispatchException(e, serializedMicroservicePayload);
                        throw;
                    }
                }
                else
                {
                    throw new Exception($"Can't acquire lock to safely trigger timer: {key}");
                }
            }
        }
    }
}
