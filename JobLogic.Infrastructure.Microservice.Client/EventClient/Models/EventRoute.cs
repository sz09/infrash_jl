using System.Collections.Generic;
using System.Linq;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public sealed class EventRoute
    {
        public Dictionary<string,string> SubscriptionNotationSuffixDict { get; set; }

        public string GetApplicableSubscriptionNotationSuffix(string subscriptionNotation)
        {
            var found = SubscriptionNotationSuffixDict?.ContainsKey(subscriptionNotation) == true;
            if (found)
            {
                return SubscriptionNotationSuffixDict[subscriptionNotation];
            }
            else
            {
                return null;
            }
        }
    }
}
