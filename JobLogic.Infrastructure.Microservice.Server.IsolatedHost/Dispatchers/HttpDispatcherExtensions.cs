using JobLogic.Infrastructure.Microservice.Client;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace JobLogic.Infrastructure.Microservice.Server.IsolatedHost
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
        public static async Task<HttpResponseData> DispatchAsync(this IDispatcher dispatcher, HttpDispatcherSetting httpDispatcherSetting, HttpRequestData req)
        {
            string serializedMicroservicePayload = await req.ReadAsStringAsync();
            try
            {
                var payload = JsonConvert.DeserializeObject<MicroservicePayload>(serializedMicroservicePayload);
                await httpDispatcherSetting.ConfigurationRefresherProvider.TryRefreshWithFirstRefresherAsync();
                var result = await dispatcher.DispatchAsync(payload);
                var resp = req.CreateResponse();
                switch(result.State)
                {
                    case InvocationResult.ResultState.Cacncelled:
                        httpDispatcherSetting.Logger.LogDispatchCancelled(result.CancelMessage, serializedMicroservicePayload);
                        resp.StatusCode = (HttpStatusCode)422;
                        await resp.WriteStringAsync(result.CancelMessage);
                        break;
                    case InvocationResult.ResultState.Success:
                        resp.StatusCode = HttpStatusCode.OK;
                        break;
                    case InvocationResult.ResultState.SuccessWithRespone:
                        resp.StatusCode = HttpStatusCode.OK;
                        var stringRs = JsonConvert.SerializeObject(result.GetResponseOnlyWhenStateIsSuccessWithRespone());
                        await resp.WriteStringAsync(stringRs);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                return resp;
            }
            catch (Exception e)
            {
                httpDispatcherSetting.Logger.LogDispatchException(e, serializedMicroservicePayload);
                var resp = req.CreateResponse(HttpStatusCode.InternalServerError);
                return resp;
            }
        }

    }
}
