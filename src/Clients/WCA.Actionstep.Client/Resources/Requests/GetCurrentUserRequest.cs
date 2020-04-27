using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class GetCurrentUserRequest : IActionstepRequest
    {
        public GetCurrentUserRequest()
        {
        }

        public GetCurrentUserRequest(TokenSetQuery tokenSetQuery)
        {
            TokenSetQuery = tokenSetQuery;
        }

        public string RelativeResourcePath => "rest/users/current";

        public HttpMethod HttpMethod => HttpMethod.Get;

        public TokenSetQuery TokenSetQuery { get; set; }

        public object JsonPayload => null;
    }
}
