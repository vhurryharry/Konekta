using System.Net.Http;

namespace WCA.PEXA.Client.Resources
{
    public class RetrieveUserProfileQuery: PEXARequestBase
    {
        public RetrieveUserProfileQuery(string bearerToken)
        {
            BearerToken = bearerToken;
        }

        public override string Path => "/v1/user";

        public override HttpMethod HttpMethod => HttpMethod.Get;

        public override HttpContent Content => null;

        public override int Version => 1;
    }
}
