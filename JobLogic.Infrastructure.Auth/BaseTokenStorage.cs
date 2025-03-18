using System;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.Auth
{
    public abstract class BaseTokenStorage: IBaseTokenStorage
    {
        private Dictionary<string, BaseAuthToken> authTokens;

        public BaseTokenStorage()
        {
            authTokens = new Dictionary<string,BaseAuthToken>();
        }

        public void Add(string companyId, BaseAuthToken authToken)
        {
            if (string.IsNullOrWhiteSpace(companyId))
            {
                throw new ArgumentNullException("companyId");
            }

            if (authTokens.ContainsKey(companyId))
            {
                authTokens[companyId] = authToken;
            }
            else
            {
                authTokens.Add(companyId, authToken);
            }
        }

        public void Remove(string companyId)
        {
            if (string.IsNullOrWhiteSpace(companyId))
            {
                throw new ArgumentNullException("companyId");
            }

            if (authTokens.ContainsKey(companyId))
            {
                authTokens.Remove(companyId);
            }
        }

        public BaseAuthToken Find(string companyId)
        {
            if (string.IsNullOrWhiteSpace(companyId))
            {
                throw new ArgumentNullException("companyId");
            }

            if (authTokens.ContainsKey(companyId))
            {
                return authTokens[companyId];
            }

            return null;
        }
    }
}
