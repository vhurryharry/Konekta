
using System.Net.Http;

namespace WCA.PEXA.Client.Resources
{
    public class RetrieveWorkgroupsQuery: PEXARequestBase
    {
        public RetrieveWorkgroupsQuery(string bearerToken)
        {
            BearerToken = bearerToken;
        }

        public override string Path => "/v1/subscriber/workgroups";

        public override HttpMethod HttpMethod => HttpMethod.Get;

        public override HttpContent Content => null;

        public override int Version => 1;
    }
}
