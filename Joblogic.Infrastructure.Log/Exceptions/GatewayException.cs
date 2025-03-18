using System;

namespace JobLogic.Infrastructure.Log
{
    public class GatewayException : BaseException
    {
        public GatewayException(string message)
            : base(message)
        {

        }
        public GatewayException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
