using System;

namespace JobLogic.Infrastructure.OData.Client
{
    public class JobLogicODataClientException : Exception
    {
        public JobLogicODataClientException(string msg = null) : base(msg)
        {

        }
    }
}
