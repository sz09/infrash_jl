using JobLogic.Infrastructure.Contract;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Event.Publisher
{
    public static class MessageExtensions
    {
        public static Message ToMessage(this MicroservicePayload payload, EventServiceBusOption serviceBusOption)
        {
            var sbmessage = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload)));
            if (serviceBusOption?.DelayMessageInMinutes > 0)
            {
                sbmessage.ScheduledEnqueueTimeUtc = DateTime.UtcNow.AddMinutes(serviceBusOption.DelayMessageInMinutes.Value);
            }
            if (serviceBusOption?.UserProperties?.Count > 0)
            {
                foreach (var v in serviceBusOption.UserProperties)
                {
                    sbmessage.UserProperties[v.Key] = v.Value;
                }
            }
            return sbmessage;
        }

        


    }
}
