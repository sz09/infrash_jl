using System;

namespace JobLogic.Infrastructure.Contract
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]  
    public class SubscribingServiceAttribute : Attribute
    {
        public SubscribingServiceAttribute(string serviceName)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; }
    }
}
