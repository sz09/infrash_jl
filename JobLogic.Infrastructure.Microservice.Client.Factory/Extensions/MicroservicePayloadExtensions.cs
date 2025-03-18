using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;

namespace JobLogic.Infrastructure.Microservice.Client.Factory
{
    public static class MicroservicePayloadExtensions
    {
        public static ServiceBusMessage ToMessage(this MicroservicePayload payload, ServiceBusOption serviceBusOption)
        {
            var sbmessage = new ServiceBusMessage(JsonConvert.SerializeObject(payload));
            if (serviceBusOption?.DelayMessageInMinutes > 0)
            {
                sbmessage.ScheduledEnqueueTime = DateTime.UtcNow.AddMinutes(serviceBusOption.DelayMessageInMinutes.Value);
            }
            if(serviceBusOption?.UserProperties?.Count > 0)
            {
                foreach(var v in serviceBusOption.UserProperties)
                {
                    sbmessage.ApplicationProperties[v.Key] = v.Value;
                }
            }
            return sbmessage;
        }

        public static ServiceBusMessage ToMessage(this MicroservicePayload payload, ServiceBusSession session, ServiceBusOption serviceBusOption)
        {
            var sbmessage = payload.ToMessage(serviceBusOption);
            var sessionId = session?.SessionId;
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new NullReferenceException(nameof(sessionId));
            }
            sbmessage.SessionId = sessionId;
            return sbmessage;
        }


        public const string SubscribingServicesNotation_PropertyKey = "SubscribingServicesNotation";
        public static ServiceBusMessage ToTopicEventMessage(this MicroservicePayload payload, EventServiceBusOption serviceBusOption, string subscribingServicesNotation)
        {

            var sbmessage = new ServiceBusMessage(JsonConvert.SerializeObject(payload));
            if (serviceBusOption?.DelayMessageInMinutes > 0)
            {
                sbmessage.ScheduledEnqueueTime = DateTime.UtcNow.AddMinutes(serviceBusOption.DelayMessageInMinutes.Value);
            }
            if (serviceBusOption?.UserProperties?.Count > 0)
            {
                foreach (var v in serviceBusOption.UserProperties)
                {
                    sbmessage.ApplicationProperties[v.Key] = v.Value;
                }
            }
            if (sbmessage.ApplicationProperties.ContainsKey(SubscribingServicesNotation_PropertyKey))
            {
                throw new Exception($"{SubscribingServicesNotation_PropertyKey} is a reserved UserProperty Key");
            }
            else
            {
                sbmessage.ApplicationProperties[SubscribingServicesNotation_PropertyKey] = subscribingServicesNotation;
            }
            return sbmessage;
        }
    }
}
