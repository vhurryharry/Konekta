using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace WCA.Core.Services
{
    public class AppInsightsTelemetryLogger : ITelemetryLogger
    {
        private TelemetryClient _telemetryClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telemetryClient">
        ///     Should be automatically registered with DI after adding app insights services.
        ///     See: https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core#how-can-i-track-telemetry-thats-not-automatically-collected
        /// </param>
        public AppInsightsTelemetryLogger(IOptions<TelemetryConfiguration> options)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));

            _telemetryClient = new TelemetryClient(options.Value);
        }

        public void TrackTrace(string message, WCASeverityLevel severityLevel, IDictionary<string, string> properties)
        {
            // We can coerce as WCASeverityLevel is a clone of SeverityLevel and has the same values.
            SeverityLevel appInsightsSeverityLevel = (SeverityLevel)severityLevel;

            _telemetryClient.TrackTrace(message, appInsightsSeverityLevel, properties);
        }

        public void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            _telemetryClient.TrackException(exception, properties, metrics);
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            _telemetryClient.TrackEvent(eventName, properties, metrics);
        }
    }
}
