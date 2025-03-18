using JobLogic.Infrastructure.Microservice.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Server.Host
{
    public class HttpDispatcherSetting
    {
        public HttpDispatcherSetting(IConfigurationRefresherProvider configurationRefresherProvider, ILogger<HttpDispatcherSetting> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ConfigurationRefresherProvider = configurationRefresherProvider ?? throw new ArgumentNullException(nameof(configurationRefresherProvider));
        }

        public IConfigurationRefresherProvider ConfigurationRefresherProvider { get; }
        public ILogger<HttpDispatcherSetting> Logger { get; }
    }
    public static class HttpDispatcherExtensions
    {
        public static async Task<IActionResult> DispatchAsync(this IDispatcher dispatcher, HttpDispatcherSetting httpDispatcherSetting, HttpRequest req)
        {
            string serializedMicroservicePayload = await req.ReadAsStringAsync();
            try
            {
                var payload = JsonConvert.DeserializeObject<MicroservicePayload>(serializedMicroservicePayload);
                await httpDispatcherSetting.ConfigurationRefresherProvider.TryRefreshWithFirstRefresherAsync();
                var result = await dispatcher.DispatchAsync(payload);

                switch(result.State)
                {
                    case InvocationResult.ResultState.Cacncelled:
                        {
                            httpDispatcherSetting.Logger.LogDispatchCancelled(result.CancelMessage, serializedMicroservicePayload);
                            return new UnprocessableEntityObjectResult(result.CancelMessage);
                        }
                    case InvocationResult.ResultState.Success:
                        return new OkResult();
                    case InvocationResult.ResultState.SuccessWithRespone:
                        return new OkObjectResult(result.GetResponseOnlyWhenStateIsSuccessWithRespone());
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception e)
            {
                httpDispatcherSetting.Logger.LogDispatchException(e, serializedMicroservicePayload);
                return new StatusCodeResult(500);
            }
        }
    }
}
