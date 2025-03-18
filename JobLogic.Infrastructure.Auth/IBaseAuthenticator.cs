using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Auth
{
    public interface IBaseAuthenticator<TAuthToken, TAuthorizationCode>
        where TAuthToken : BaseAuthToken
        where TAuthorizationCode : BaseAuthorizationCode
    {
        [Obsolete("Should use Async version instead")]
        TAuthToken GetRequestToken();
        Task<TAuthToken> GetRequestTokenAsync(CancellationToken cancellationToken = default);

        [Obsolete("Should use Async version instead")]
        TAuthToken GetAccessToken(TAuthorizationCode authorizationCode);

        Task<TAuthToken> GetAccessTokenAsync(TAuthorizationCode authorizationCode, CancellationToken cancellationToken = default);

        string GetAuthorizationUrl(TAuthToken requestToken);
    }
}
