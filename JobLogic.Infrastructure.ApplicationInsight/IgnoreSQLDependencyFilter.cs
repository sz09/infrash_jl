using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace JobLogic.Infrastructure.ApplicationInsight
{
    public class IgnoreSQLDependencyFilter : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }

        // Link processors to each other in a chain.
        public IgnoreSQLDependencyFilter(ITelemetryProcessor next)
        {
            this.Next = next;
        }
        public void Process(ITelemetry item)
        {
            if (IsSQLDependency(item)) { return; }
            this.Next.Process(item);
        }

        private bool IsSQLDependency(ITelemetry item)
        {
            var dependency = item as DependencyTelemetry;
            if (dependency?.Type == "SQL")
            {
                return true;
            }
            return false;
        }
    }
}
