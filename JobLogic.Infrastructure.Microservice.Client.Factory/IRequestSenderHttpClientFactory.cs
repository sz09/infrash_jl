using System.Net.Http;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public interface IRequestSenderHttpClientFactory
    {
        HttpClient HttpClientFor(RequestAddress requestAddress);
    }
    
}
