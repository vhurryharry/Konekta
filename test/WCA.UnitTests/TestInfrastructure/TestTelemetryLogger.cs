using System;
using System.Collections.Generic;
using WCA.Core.Services;

namespace WCA.UnitTests.TestInfrastructure
{
    public class TestTelemetryLogger : ITelemetryLogger
    {
        public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            // Swallow messages for testing
        }

        public void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            // Swallow messages for testing
        }

        public void TrackTrace(string message, WCASeverityLevel severityLevel, IDictionary<string, string> properties)
        {
            // Swallow messages for testing
        }
    }
}
