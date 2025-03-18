using System;

namespace JobLogic.Infrastructure.Log
{
    public abstract class BaseException : SystemException
    {
        public BaseException(string message)
            : base(message)
        {
        }
        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
