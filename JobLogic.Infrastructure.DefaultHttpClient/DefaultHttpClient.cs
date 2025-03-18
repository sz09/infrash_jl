using System;
using System.Net.Http;

namespace JobLogic.Infrastructure.DefaultHttpClient
{
    public static class DefaultHttpClient
    {
        static DefaultHttpClient()
        {
#if NETCOREAPP
            Instance = CreateForNetCore();
#else
            Instance = CreateForNetStd();
#endif
        }
#if NETCOREAPP
        static SocketsHttpHandler _sockethandler = new SocketsHttpHandler()
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(15)
        };
        static IHttpClient CreateForNetCore()
        {
            var client = new InternalHttpClient(_sockethandler, disposeHandler: false);
            client.DefaultRequestHeaders.Add("User-Agent", "Net Core DefaultHttpClient");
            return client;
        }
#else
        static IHttpClient CreateForNetStd()
        {
            var client = new InternalHttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Net Standard DefaultHttpClient");
            return client;
        }
#endif

        public static IHttpClient Instance { get; }
    }
}
