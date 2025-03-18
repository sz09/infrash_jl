using System;

namespace JobLogic.Infrastructure.Microservice.Client.Factory
{
    public class MicroserviceClientException : Exception
    {
        public MicroserviceClientException(string message) : base(message)
        {

        }
    }
}
