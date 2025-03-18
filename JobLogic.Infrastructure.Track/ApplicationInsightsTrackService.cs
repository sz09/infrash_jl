using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobLogic.Infrastructure.Track
{
    public interface ITrackService
    {
        void TrackEvent(string eventName, object properties = null, IDictionary<string, double> metrics = null);
        void TrackException(Exception exception, object properties = null, IDictionary<string, double> metrics = null);
        void TraceInformation(string message, object properties = null);
        void TraceWarning(string message, object properties = null);
        void TraceError(string message, object properties = null);
        void TraceCritical(string message, object properties = null);
    }
    public class ApplicationInsightsTrackService : ITrackService
    {
        const string CUSTOM_DIMENSION_DEFAULT_PREFIX = "_JL.";
        //DFCD stand for "Default custom dimension"
        public const string DFCD_MESSAGE = CUSTOM_DIMENSION_DEFAULT_PREFIX + "Message";
        

        private readonly TelemetryClient _telemetryClient;

        /// <summary>
        /// Constructor using "Convention over configuration", using appSetting APPINSIGHTS_INSTRUMENTATIONKEY
        /// </summary>
        public ApplicationInsightsTrackService()
        {
            _telemetryClient = new TelemetryClient();
        }

        public ApplicationInsightsTrackService(string applicationInsightsInstrumentationKey, IList<ITelemetryInitializer> initializers = null) : this()
        {
            var telemetryConfig = TelemetryConfiguration.Active;
            if (string.IsNullOrEmpty(telemetryConfig.InstrumentationKey))
            {
                if (!string.IsNullOrEmpty(applicationInsightsInstrumentationKey))
                {
                    telemetryConfig.InstrumentationKey = applicationInsightsInstrumentationKey;

                    if (initializers?.Any() == true)
                    {
                        foreach (var item in initializers)
                        {
                            telemetryConfig.TelemetryInitializers.Add(item);
                        }
                    }              
                }

            };
        }

        public void TrackEvent(string eventName, object properties = null, IDictionary<string, double> metrics = null)
        {
            var customDimension = properties.MapToAppInsightCustomDimentionDataType();
            _telemetryClient.TrackEvent(eventName, customDimension, metrics);
           // _telemetryClient.Flush();
        }

        public void TrackException(Exception exception, object properties = null, IDictionary<string, double> metrics = null)
        {
            var customDimension = properties.MapToAppInsightCustomDimentionDataType();
            _telemetryClient.TrackException(exception, customDimension, metrics);
            //_telemetryClient.Flush();
        }

        public void TraceInformation(string message, object properties = null)
        {
            TrackTrace(message, SeverityLevel.Information, properties);
        }

        public void TraceWarning(string message, object properties = null)
        {
            TrackTrace(message, SeverityLevel.Warning, properties);
        }

        public void TraceError(string message, object properties = null)
        {
            TrackTrace(message, SeverityLevel.Error, properties);
        }

        public void TraceCritical(string message, object properties = null)
        {
            TrackTrace(message, SeverityLevel.Critical, properties);
        }

        private void TrackTrace(string message, SeverityLevel severityLevel, object properties)
        {
            var customDimension = properties.MapToAppInsightCustomDimentionDataType();
            _telemetryClient.TrackTrace(message, severityLevel, customDimension);
            //_telemetryClient.Flush();
        }
    }
}
