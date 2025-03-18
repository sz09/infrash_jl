//using JobLogic.AccountIntegration.Logger;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Auth
{
    public abstract class BaseAuthenticator<TAuthToken, TAuthConfiguration, TAuthorizationCode> : IBaseAuthenticator<TAuthToken, TAuthorizationCode>
        where TAuthToken : BaseAuthToken
        where TAuthConfiguration : BaseAuthConfiguration
        where TAuthorizationCode : BaseAuthorizationCode
    {
        protected readonly TAuthConfiguration authConfiguration;
        
        protected virtual string Authenticator { get; }

        public BaseAuthenticator(TAuthConfiguration authConfiguration)
        {
            this.authConfiguration = authConfiguration;
        }

        [Obsolete("Should use Async version instead")]
        public abstract TAuthToken GetRequestToken();

        public abstract Task<TAuthToken> GetRequestTokenAsync(CancellationToken cancellationToken = default);

        [Obsolete("Should use Async version instead")]
        public abstract TAuthToken GetAccessToken(TAuthorizationCode authorizationCode);

        public abstract Task<TAuthToken> GetAccessTokenAsync(TAuthorizationCode authorizationCode, CancellationToken cancellationToken = default);

        public abstract string GetAuthorizationUrl(TAuthToken requestToken);
    }
}
