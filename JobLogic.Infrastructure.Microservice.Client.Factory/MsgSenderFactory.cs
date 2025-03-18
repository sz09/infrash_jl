using JobLogic.Infrastructure.Microservice.Client.Factory;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public class MsgSenderFactory : IMsgSenderFactory
    {
        class DefaultRequestSenderHttpClientFactory : IRequestSenderHttpClientFactory
        {
            static ConcurrentDictionary<string, HttpClient> HttpClients = new ConcurrentDictionary<string, HttpClient>();
            public HttpClient HttpClientFor(RequestAddress requestAddress)
            {
                return HttpClients.GetOrAdd(requestAddress.Origin, origin =>
                {
                    return new HttpClient() { Timeout = TimeSpan.FromSeconds(210) };
                });
            }
        }

        public IRequestSender GetRequestSender()
        {
            return new RequestSender(new DefaultRequestSenderHttpClientFactory());
        }

        public ICommandSender GetCommandSender()
        {
            return new CommandSender();
        }

        public ICommandSessionSender GetCommandSessionSender()
        {
            return new CommandSessionSender();
        }
    }
}
