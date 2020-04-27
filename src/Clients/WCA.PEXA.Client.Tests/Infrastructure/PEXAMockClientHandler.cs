using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WCA.PEXA.Client.Tests.Infrastructure
{
    public abstract class PEXAMockClientHandler : HttpClientHandler
    {
        public bool ReturnException { get; set; }
        protected override  Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (ReturnException)
                return Task.FromResult(SendAsync(HttpMethod.Get, "/exception-response"));
            else
                return Task.FromResult(SendAsync(request.Method, request.RequestUri.PathAndQuery));
        }

#pragma warning disable CA1054 // Uri parameters should not be strings
        public abstract HttpResponseMessage SendAsync(HttpMethod method, string url);
#pragma warning restore CA1054 // Uri parameters should not be strings
    }
}
