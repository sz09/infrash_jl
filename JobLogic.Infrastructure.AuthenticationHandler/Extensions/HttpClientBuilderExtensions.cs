using System;
using System.Net.Http;
using JobLogic.Infrastructure.AuthenticationHandler;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ***************************************************************************************
// DO NOT CHANGE THE NAMESPACE! It is .Net Core convention for configuration extensions. *
// ***************************************************************************************
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddAuthentication(this IHttpClientBuilder builder,
            Func<IServiceProvider, ClientCredentials> credentialsProvider,
            Func<IServiceProvider, string> identityAuthorityProvider, Func<IServiceProvider, string> tokenUriProvider)
        {
            builder.Services.TryAddSingleton<AccessTokensCacheManager>();
            builder.AddHttpMessageHandler(provider =>
            {
                var credentials = credentialsProvider.Invoke(provider);
                var identityAuthority = identityAuthorityProvider.Invoke(provider);
                var tokenEndpoint = tokenUriProvider.Invoke(provider);

                return CreateDelegatingHandler(provider, credentials, identityAuthority, tokenEndpoint);
            });

            return builder;
        }

        public static IHttpClientBuilder AddAuthentication(this IHttpClientBuilder builder, ClientCredentials credentials, string identityAuthority, string tokenUri)
        {
            builder.Services.TryAddSingleton<AccessTokensCacheManager>();
            builder.AddHttpMessageHandler(provider => CreateDelegatingHandler(provider, credentials, identityAuthority, tokenUri));
            
            return builder;
        }

        private static AuthenticationDelegatingHandler CreateDelegatingHandler(IServiceProvider provider, 
            ClientCredentials credentials, string identityAuthority, string tokenUri)
        {
            var httpClient = CreateHttpClient(provider, identityAuthority);
            var accessTokensCacheManager = provider.GetRequiredService<AccessTokensCacheManager>();

            return new AuthenticationDelegatingHandler(accessTokensCacheManager, credentials, httpClient, tokenUri);
        }

        private static HttpClient CreateHttpClient(IServiceProvider provider, string identityAuthority)
        {
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(identityAuthority);

            return httpClient;
        }
    }
}
