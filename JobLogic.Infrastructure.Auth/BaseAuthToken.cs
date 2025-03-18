using System;

namespace JobLogic.Infrastructure.Auth
{
    public abstract class BaseAuthToken
    {
        public BaseAuthToken()
        {

        }

        public abstract bool IsExpired { get; }

        public abstract DateTime? ExpiredTime { get; }

        public abstract string OrganisationName { get; set; }

        public abstract string Currency { get; set; }

        public abstract string UserId { get; set; }
    }
}
