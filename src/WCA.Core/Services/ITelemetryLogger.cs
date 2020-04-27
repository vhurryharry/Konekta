using System;
using System.Collections.Generic;

namespace WCA.Core.Services
{
    public interface ITelemetryLogger
    {
        void TrackTrace(string message, WCASeverityLevel severityLevel, IDictionary<string, string> properties);

        void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);
    }
}
