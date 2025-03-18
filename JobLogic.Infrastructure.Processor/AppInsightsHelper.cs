using JobLogic.Infrastructure.Track;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace JobLogic.Infrastructure.Processor
{
    public static class AppInsightsHelper
    {
        static object _lockObject = new object();
        static TelemetryConfiguration _telemetryConfig = null;
        private static DependencyTrackingTelemetryModule InitializeDependencyTracking(string applicationInsightsInstrumentationKey, string applicationInsightsIgnoreSqlDependency)
        {
            lock (_lockObject)
            {
                if (_telemetryConfig == null)
                {
                    _telemetryConfig = TelemetryConfiguration.Active;
                    if (string.IsNullOrEmpty(_telemetryConfig.InstrumentationKey))
                    {
                        if (!string.IsNullOrEmpty(applicationInsightsInstrumentationKey))
                            _telemetryConfig.InstrumentationKey = applicationInsightsInstrumentationKey;
                    }
                    _telemetryConfig.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());
                    _telemetryConfig.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
                    if (applicationInsightsIgnoreSqlDependency == "1")
                    {
                        var builder = _telemetryConfig.TelemetryProcessorChainBuilder;
                        builder.Use((next) => new IgnoreSQLDependencyFilter(next));
                        builder.Build();
                    }
                }
            }

            var module = new DependencyTrackingTelemetryModule();
            module.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.windows.net");
            module.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.chinacloudapi.cn");
            module.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.cloudapi.de");
            module.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.usgovcloudapi.net");

            module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
            module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.EventHubs");

            module.Initialize(_telemetryConfig);

            return module;
        }

        public static void ExecuteInOperationContext(string processorName, string applicationInsightsInstrumentationKey, string applicationInsightsIgnoreSqlDependency, Action backgroundOperation)
        {
            using (InitializeDependencyTracking(applicationInsightsInstrumentationKey, applicationInsightsIgnoreSqlDependency))
            {
                var tc = new TelemetryClient();
                
                var requestActivity = new System.Diagnostics.Activity(processorName);
                requestActivity.Start();
                var requestOperation = tc.StartOperation<RequestTelemetry>(requestActivity);
                try
                {
                    tc.TrackTrace($"App Started: {processorName}");
                    tc.Flush();
                    backgroundOperation?.Invoke();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    tc.StopOperation(requestOperation);
                    tc.Flush();
                }
            }
        }
    }
}
