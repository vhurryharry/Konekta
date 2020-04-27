using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WCA.Actionstep.Client.Tests
{
    public abstract class MockHandler : HttpClientHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var headers = request.Headers;
            if (headers.Authorization != null && !string.IsNullOrEmpty(headers.Authorization.Parameter))
            {
                if (headers.Authorization.Parameter.Contains("expired", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Task.FromResult(SendAsync(HttpMethod.Get, "expired-token"));
                }

                if (headers.Authorization.Parameter.Contains("invalid", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Task.FromResult(SendAsync(HttpMethod.Get, "invalid-token"));
                }
            }

            return Task.FromResult(SendAsync(request.Method, request.RequestUri.ToString()));
        }

#pragma warning disable CA1054 // Uri parameters should not be strings
        public abstract HttpResponseMessage SendAsync(HttpMethod method, string url);
#pragma warning restore CA1054 // Uri parameters should not be strings
    }
}
