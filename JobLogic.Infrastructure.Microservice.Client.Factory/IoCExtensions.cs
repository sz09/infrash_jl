using JobLogic.Infrastructure.DistributedCache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Polly;
using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Authentication;

namespace JobLogic.Infrastructure.Microservice.Client.Factory
{
    public static class IoCExtensions
    {
        public static void AddDefaultMsgSenderWithRetryFactory(this IServiceCollection services, int timeoutInSeconds = 210, IAsyncPolicy<HttpResponseMessage> retryPolicy = null)
        {
            retryPolicy = retryPolicy ?? GetDefaultRetryPolicy(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt / 2)));

            services.AddTransient<DefaultMsgSenderWithRetryFactory>();
            services.AddTransient<DefaultMsgSenderWithRetryFactory.RequestSenderHttpClientFactory>();

            services.AddHttpClient(DefaultMsgSenderWithRetryFactory.RequestSenderHttpClientFactory.HTTP_CLIENT_NAME,
                x =>
                {
                    x.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
                })
                .AddPolicyHandler(retryPolicy);
        }

        public static IMsgSenderFactory GetDefaultMsgSenderWithRetryFactory(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<DefaultMsgSenderWithRetryFactory>();
        }

        public static IAsyncPolicy<HttpResponseMessage> GetDefaultRetryPolicy(int maxRetryTimes, Func<int, TimeSpan> sleepDurationProvider)
        {
            System.Net.HttpStatusCode tooManyRequest = (System.Net.HttpStatusCode)429;
            return Policy
                .Handle<HttpRequestException>()
                .Or<SocketException>()
                .Or<AuthenticationException>()
                .OrResult<HttpResponseMessage>(x => x.StatusCode == System.Net.HttpStatusCode.NotFound ||
                                                      x.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable ||
                                                      x.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                                                      x.StatusCode == System.Net.HttpStatusCode.BadGateway ||
                                                      x.StatusCode == tooManyRequest)
                .WaitAndRetryAsync(maxRetryTimes, sleepDurationProvider);
        }

        public static void AddCachetserter(this IServiceCollection services, Func<IServiceProvider, IDistributedCacheClient> distributedCacheClientFunc, bool ignoreMissingLogger = false)
        {
            services.AddTransient(x =>
            {
                ILogger<Cachetserter> logger;
                if (ignoreMissingLogger)
                {
                    logger = x.GetService<ILogger<Cachetserter>>() ?? new NullLogger<Cachetserter>();
                }
                else
                {
                    logger = x.GetRequiredService<ILogger<Cachetserter>>();
                }
                var distributedCacheClient = distributedCacheClientFunc(x);
                return new Cachetserter(distributedCacheClient, logger);
            });
        }
        public static ICachetserter GetCachetserter(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<Cachetserter>();
        }
    }
}
