using System;

namespace JobLogic.Infrastructure.Log
{
    public class ExpiredTokenException: BaseException
    {
        
        public ExpiredTokenException(string message)
            :base(message)
        {

        }
        public ExpiredTokenException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
