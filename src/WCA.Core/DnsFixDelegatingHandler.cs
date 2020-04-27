using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WCA.Core
{
    /// <summary>
    /// Sets connection timeout on endpoints to avoid stale DNS issues as per:
    /// <see cref="https://byterot.blogspot.com/2016/07/singleton-httpclient-dns.html"/>
    ///
    /// IMPORTANT: This DelegatingHandler must be after other handlers that modify the RequestUri.
    /// </summary>
    /// <seealso cref="WCA.Core.Features.Actionstep.DelegatingHandler" />
    public class DnsFixDelegatingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sp = ServicePointManager.FindServicePoint(request.RequestUri);
            sp.ConnectionLeaseTimeout = 60 * 1000; // 1 minute
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
