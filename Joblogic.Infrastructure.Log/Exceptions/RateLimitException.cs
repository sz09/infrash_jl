using System;

namespace JobLogic.Infrastructure.Log
{
    public class RateLimitException : BaseException
    {
        public RateLimitException(string message)
            : base(message)
        {

        }
        public RateLimitException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public string RateLimitProblem { get; set; }
    }
}
