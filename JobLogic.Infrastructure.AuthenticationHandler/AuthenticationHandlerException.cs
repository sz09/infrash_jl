using System;

namespace JobLogic.Infrastructure.AuthenticationHandler
{
    public class AuthenticationHandlerException : Exception
    {
        public AuthenticationHandlerException(string message) : base(message)
        {
        }
    }
}