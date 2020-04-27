using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WCA.Core.Logging
{
    public class HttpClientAppInsightsHandler : DelegatingHandler
    {
        private readonly TelemetryClient _telemetryClient;

        public HttpClientAppInsightsHandler(IOptions<TelemetryConfiguration> options)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));

            _telemetryClient = new TelemetryClient(options.Value);
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            try
            {
                if (request != null && response != null && (_telemetryClient?.IsEnabled() ?? false))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var telemetry = new DependencyTelemetry();
                        telemetry.Type = "Http";
                        telemetry.Data = await request.Content?.ReadAsStringAsync();
                        telemetry.Name = $"{request.Method.Method} {request.RequestUri.AbsolutePath}";
                        telemetry.Target = request.RequestUri?.ToString();
                        telemetry.ResultCode = response.StatusCode.ToString();
                        telemetry.Properties.Add("Telemetry Source", "HttpClientAppInsightsHandler");
                        telemetry.Properties.Add("Request Headers", request.Headers.ToString());
                        telemetry.Properties.Add("Request Content Headers", request.Content.Headers.ToString());
                        telemetry.Properties.Add("Response Headers", response.Headers.ToString());
                        telemetry.Properties.Add("Response Body", await response.Content?.ReadAsStringAsync());
                        telemetry.Success = false;
                        _telemetryClient.TrackDependency(telemetry);
                    }
                }
            }
            catch
            {
                // Swallow as we don't want this to break anything.
            }

            return response;
        }
    }
}