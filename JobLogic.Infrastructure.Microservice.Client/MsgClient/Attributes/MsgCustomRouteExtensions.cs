using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JobLogic.Infrastructure.Microservice.Client
{

    static class MsgCustomRouteExtensions
    {
        static ConcurrentDictionary<Type, MsgCustomRouteAttribute> _cache = new ConcurrentDictionary<Type, MsgCustomRouteAttribute>();
        public static string GetCustomRouteName(this BaseMicroserviceMsg msg)
        {
            var objType = msg.GetType();
            var route = _cache.GetOrAdd(objType, ot =>
            {
                return ot.GetCustomAttribute<MsgCustomRouteAttribute>(false);
            });
            return route?.CustomRouteName;
        }
    }
}
