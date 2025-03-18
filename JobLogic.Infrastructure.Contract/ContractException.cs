using System;
using System.Collections.Generic;
using System.Text;

namespace JobLogic.Infrastructure.Contract
{
    class ContractException : Exception
    {
        public ContractException(string message) : base(message) { }
    }
}
