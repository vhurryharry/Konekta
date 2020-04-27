using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class GetActionRequest : IActionstepRequest
    {
        public GetActionRequest()
        {
        }

        public GetActionRequest(TokenSetQuery tokenSetQuery, int actionId)
        {
            TokenSetQuery = tokenSetQuery;
            ActionId = actionId;
        }

        public int ActionId { get; set; }

        public TokenSetQuery TokenSetQuery { get; set; }

        public string RelativeResourcePath =>
            $"rest/actions?id_eq={ActionId}&include=actionType,division,division.participant";

        public HttpMethod HttpMethod => HttpMethod.Get;

        public object JsonPayload => null;
    }
}
