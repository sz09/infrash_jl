using JobLogic.Infrastructure.Microservice.Client.Factory;
using System.Collections.Concurrent;
using System.Net.Http;

namespace JobLogic.Infrastructure.Microservice.Client
{
    internal class DefaultMsgSenderWithRetryFactory : IMsgSenderFactory
    {
        private readonly RequestSenderHttpClientFactory _requestSenderHttpClientFactory;
        public class RequestSenderHttpClientFactory : IRequestSenderHttpClientFactory
        {
            public const string HTTP_CLIENT_NAME = "JobLogic.Infrastructure.Microservice.Client.DefaultMsgSenderWithRetryFactory.RequestSenderHttpClientFactory";
            private readonly IHttpClientFactory _httpClientFactory;
            static ConcurrentDictionary<string, HttpClient> _httpClients = new ConcurrentDictionary<string, HttpClient>();
            public RequestSenderHttpClientFactory(IHttpClientFactory httpClientFactory)
            {
                _httpClientFactory = httpClientFactory;
            }
            public HttpClient HttpClientFor(RequestAddress requestAddress)
            {
                return _httpClients.GetOrAdd(requestAddress.Origin, origin =>
                {
                    return _httpClientFactory.CreateClient(HTTP_CLIENT_NAME);
                });
            }
        }
        public DefaultMsgSenderWithRetryFactory(RequestSenderHttpClientFactory requestSenderHttpClientFactory)
        {
            _requestSenderHttpClientFactory = requestSenderHttpClientFactory;
        }

        public IRequestSender GetRequestSender()
        {
            return new RequestSender(_requestSenderHttpClientFactory);
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
