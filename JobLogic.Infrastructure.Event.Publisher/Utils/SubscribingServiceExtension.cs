using JobLogic.Infrastructure.Contract;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace JobLogic.Infrastructure.Event.Publisher
{
    public static class SubscribingServiceExtension
    {
        static ConcurrentDictionary<Type, string> ServiceNameDict = new ConcurrentDictionary<Type, string>();
        private static string PrivateGetEventSubscribingServicesNotation(object obj)
        {
            var objType = obj.GetType();
            return ServiceNameDict.GetOrAdd(objType, ot =>
            {
                var attrs = (SubscribingServiceAttribute[])Attribute.GetCustomAttributes(ot, typeof(SubscribingServiceAttribute));
                if (attrs?.Length > 0)
                {
                    var subscribingServices = attrs.Select(x => x.ServiceName);
                    return $"|{string.Join("|", subscribingServices)}|";
                }
                else
                {
                    throw new Exception("Event must have SubscribingServiceAttribute to specify the target services");
                }
            });
        }
        public static string GetEventSubscribingServicesNotation(this TenancyEvent obj) => PrivateGetEventSubscribingServicesNotation(obj);
        public static string GetEventSubscribingServicesNotation(this TenantlessEvent obj) => PrivateGetEventSubscribingServicesNotation(obj);
    }
}
