using System;
using System.Collections.Concurrent;

namespace JobLogic.Infrastructure.Microservice.Client
{
    static class SubscribingServiceExtension
    {
        static ConcurrentDictionary<Type, SubscribingServiceAttribute[]> ServiceNameDict = new ConcurrentDictionary<Type, SubscribingServiceAttribute[]>();
        public static SubscribingServiceAttribute[] GetEventSubscribingServiceAttributes(this BaseMicroserviceEvent obj)
        {
            var objType = obj.GetType();
            return ServiceNameDict.GetOrAdd(objType, ot =>
            {
                var attrs = (SubscribingServiceAttribute[])Attribute.GetCustomAttributes(ot, typeof(SubscribingServiceAttribute), false);
                if (attrs?.Length > 0)
                {
                    return attrs;
                }
                else
                {
                    throw new Exception("Event must have SubscribingServiceAttribute to specify the target services");
                }
            });
        }
    }
}
