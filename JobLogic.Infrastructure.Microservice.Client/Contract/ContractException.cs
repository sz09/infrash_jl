using System;

namespace JobLogic.Infrastructure.Microservice.Client
{
    class ContractException : Exception
    {
        public ContractException(string message) : base(message) { }
    }
}
