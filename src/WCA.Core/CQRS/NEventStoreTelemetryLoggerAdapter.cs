using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;
using NEventStore.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using LogLevel = NEventStore.Logging.LogLevel;

namespace WCA.Core.CQRS
{
    public class NEventStoreTelemetryLoggerAdapter : NEventStoreBaseLogger
    {
        private TelemetryClient _telemetryClient;
        private readonly Dictionary<string, string> _logProperties;

        public NEventStoreTelemetryLoggerAdapter(IOptions<TelemetryConfiguration> telemetryOptions, Type typeToLog, LogLevel logLevel)
            : base(logLevel)
        {
            if (telemetryOptions is null) throw new ArgumentNullException(nameof(telemetryOptions));
            if (typeToLog is null) throw new ArgumentNullException(nameof(typeToLog));

            _telemetryClient = new TelemetryClient(telemetryOptions.Value);

            _logProperties = new Dictionary<string, string>()
            {
                { "Source Type", typeToLog.FullName }
            };
        }

        public override void Debug(string message, params object[] values)
        {
            _telemetryClient.TrackTrace(GetFormattedMessage(message, values), SeverityLevel.Verbose, _logProperties);
        }

        public override void Error(string message, params object[] values)
        {
            _telemetryClient.TrackTrace(GetFormattedMessage(message, values), SeverityLevel.Error, _logProperties);
        }

        public override void Fatal(string message, params object[] values)
        {
            _telemetryClient.TrackTrace(GetFormattedMessage(message, values), SeverityLevel.Critical, _logProperties);
        }

        public override void Info(string message, params object[] values)
        {
            _telemetryClient.TrackTrace(GetFormattedMessage(message, values), SeverityLevel.Information, _logProperties);
        }

        public override void Verbose(string message, params object[] values)
        {
            _telemetryClient.TrackTrace(GetFormattedMessage(message, values), SeverityLevel.Verbose, _logProperties);
        }

        public override void Warn(string message, params object[] values)
        {
            _telemetryClient.TrackTrace(GetFormattedMessage(message, values), SeverityLevel.Warning, _logProperties);
        }

        private static string GetFormattedMessage(string message, object[] values)
        {
            return (values == null || values.Length == 0)
                            ? message
                            : string.Format(CultureInfo.InvariantCulture, message, values);
        }
    }
}
