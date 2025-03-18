using System;

namespace JobLogic.Infrastructure.Microservice.Client
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]  
    public class SubscribingServiceAttribute : Attribute
    {
        public SubscribingServiceAttribute(string serviceName)
        {
            SubscriptionNotation = serviceName;
        }

        public string SubscriptionNotation { get; }
        public string CustomRouteName { get; set; }
    }
}
