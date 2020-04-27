using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WCA.UnitTests.TestInfrastructure
{
    public abstract class MockHandler : HttpClientHandler
    {
        public string RequestContent { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new System.ArgumentNullException(nameof(request));
            }

            if (request.Content != null)
            {
                RequestContent = await request.Content.ReadAsStringAsync();
            }

            return SendAsync(request.Method, request.RequestUri.PathAndQuery);
        }

#pragma warning disable CA1054 // Uri parameters should not be strings
        public abstract HttpResponseMessage SendAsync(HttpMethod method, string url);
#pragma warning restore CA1054 // Uri parameters should not be strings
    }
}
