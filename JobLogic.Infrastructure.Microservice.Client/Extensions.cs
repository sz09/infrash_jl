using System.Collections.Generic;

namespace JobLogic.Infrastructure.Microservice.Client
{
    static class Extensions
    {
        public static MsgCustomRoute GetCustomRouteOrNULL(this IReadOnlyDictionary<string, MsgCustomRoute> customRoutes, string routeName)
        {
            if (!string.IsNullOrEmpty(routeName) && customRoutes?.ContainsKey(routeName) == true)
            {
                return customRoutes[routeName];
            }
            return null;
        }

        public static EventRoute GetCustomRouteOrNULL(this IReadOnlyDictionary<string, EventRoute> customRoute, string routeName)
        {
            if (!string.IsNullOrEmpty(routeName) && customRoute?.ContainsKey(routeName) == true)
            {
                return customRoute[routeName];
            }
            return null;
        }

        public static int IndexOfOccurence(this string s, string match, int occurence)
        {
            int startlocation = 0;
            for (int i = 1; i <= occurence; i++)
            {
                var index = s.IndexOf(match, startlocation);
                if (index >= 0)
                {
                    if (i == occurence)
                        return index;
                    startlocation = index + 1;
                }
                else
                {
                    break;
                }
            }
            return -1;
        }
    }

    public static class EnvironmentVariableName
    {
        public const string JL_PREFERRED_ROUTE = "JL_PREFERRED_ROUTE";
    }
}
