using JobLogic.Infrastructure.AuthenticationHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuthenticationHandler.UnitTests
{
    [TestClass]
    public class AccessTokensCacheManagerTest
    {
        private readonly AccessTokensCacheManager _cacheManager;

        public AccessTokensCacheManagerTest()
        {
            _cacheManager = new AccessTokensCacheManager();
        }

        [TestMethod]
        public void GetToken_EmptyCache_Null()
        {
            var token = _cacheManager.GetToken("first");
            Assert.IsNull(token);
        }

        [TestMethod]
        public void AddOrUpdate_ThenGetSameClientId_NotNull()
        {
            const string client = "client1";
            var originalToken = GenerateToken(120);
            _cacheManager.AddOrUpdateToken(client, originalToken);

            var token = _cacheManager.GetToken(client);

            Assert.AreEqual(originalToken, token);
        }

        [TestMethod]
        public void AddOrUpdate_SameClientIdTwice_GetReturnsLatest()
        {
            const string client = "client1";

            var originalToken1 = GenerateToken(120);
            _cacheManager.AddOrUpdateToken(client, originalToken1);

            var originalToken2 = GenerateToken(240);
            _cacheManager.AddOrUpdateToken(client, originalToken2);

            var token = _cacheManager.GetToken(client);

            Assert.AreEqual(originalToken2, token);
        }

        [TestMethod]
        public void AddOrUpdate_ThenGetDifferentClientId_Null()
        {
            const string client1 = "client1";
            const string client2 = "client2";
            var originalToken = GenerateToken(120);
            _cacheManager.AddOrUpdateToken(client1, originalToken);

            var token = _cacheManager.GetToken(client2);

            Assert.IsNull(token);
        }

        [TestMethod]
        public void Get_TokenExpired_Null()
        {
            const string client1 = "client1";
            var originalToken = GenerateToken(-120);
            _cacheManager.AddOrUpdateToken(client1, originalToken);

            var token = _cacheManager.GetToken(client1);

            Assert.IsNull(token);
        }

        [TestMethod]
        public void AddOrUpdate_DifferentClientIds_GetDifferentTokens()
        {
            const string client1 = "client1";
            const string client2 = "client2";

            var originalToken1 = GenerateToken(120);
            _cacheManager.AddOrUpdateToken(client1, originalToken1);

            var originalToken2 = GenerateToken(240);
            _cacheManager.AddOrUpdateToken(client2, originalToken2);

            var token1 = _cacheManager.GetToken(client1);
            Assert.AreEqual(originalToken1, token1);

            var token2 = _cacheManager.GetToken(client2);
            Assert.AreEqual(originalToken2, token2);
        }

        private TokenResponse GenerateToken(long expirationInSeconds)
        {
            var token = new TokenResponse
            {
                Scheme = "Bearer",
                AccessToken = "blah-blah",
                ExpirationInSeconds = expirationInSeconds
            };

            return token;
        }
    }
}
