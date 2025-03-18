using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace JobLogic.Infrastructure.ApplicationInsight
{
    public class UnwantedTelemetryFilter : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }

        public UnwantedTelemetryFilter(ITelemetryProcessor next)
        {
            this.Next = next;
        }

        public void Process(ITelemetry item)
        {
            var request = item as RequestTelemetry;
            if (request != null && !string.IsNullOrEmpty(request.Name))
            {
                if (request.Name.Contains(@"/signalr/"))
                {
                    return;
                }

                if (request.Name == @"GET /" && request.Context != null)
                {
                    var userContext = request.Context.User;
                    var sessionContext = request.Context.Session;
                    if (string.IsNullOrEmpty(userContext?.Id) && string.IsNullOrEmpty(sessionContext?.Id))
                    {
                        return;
                    }
                }
            }

            // Send everything else:
            this.Next.Process(item);
        }
    }
}
