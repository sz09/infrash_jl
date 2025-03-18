using System.Collections.Generic;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public class ServiceBusOption
    {
        public ServiceBusOption()
        {
            UserProperties = new Dictionary<string, object>();
        }
        public int? DelayMessageInMinutes { get; set; }
        public IDictionary<string,object> UserProperties { get; }
    }
}
