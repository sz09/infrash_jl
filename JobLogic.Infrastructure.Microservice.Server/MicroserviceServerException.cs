using System;

namespace JobLogic.Infrastructure.Microservice.Server
{
    class MicroserviceServerException : Exception
    {
        public MicroserviceServerException(string message) : base(message) { }
    }
}
