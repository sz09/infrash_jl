namespace JobLogic.InfrastructureCore.IdentityServer.Models
{
    public static class IdentityServerConstants
    {
        public static class Errors
        {
            public const string UnsupportedGrantType = "unsupported_grant_type";
            public const string InvalidClient = "invalid_client";
            public const string InvalidScope = "invalid_scope";
            public const string InvalidGrant = "invalid_grant";
        }

        public static class ErrorDescriptions
        {
            public const string InvalidUsernameOrPassword = "invalid_username_or_password";
        }
        public static class MFAProvider
        {
            public const string Email = "Email";
            public const string Phone = "Phone";
            public const string JLEmail = "JLEmailOTP";
        }
        public static class JLClaimType
        {
            public const string JLApplication = "jl_applications";
            public const string JLMFA = "mfa";
            public const string JLMFAProvider = "mfa_provider";
        }
        public static class JLClaimValue
        {
            public const string JLWeb = "jl_web";
            public const string JLPortal = "jl_portal";
            public const string JLMobile = "jl_mobile";
            public const string Budweiser = "budweiser_portal";
        }
    }
}
