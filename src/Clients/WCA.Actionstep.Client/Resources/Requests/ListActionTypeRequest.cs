using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class ListActionTypeRequest : IActionstepRequest
    {
        public int? Id { get; set; }

        public TokenSetQuery TokenSetQuery { get; set; }

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string RelativeResourcePath =>
                Id is null
                    ? "rest/actiontypes"
                    : $"rest/actiontypes?id_in={Id}";

        public object JsonPayload => null;
    }
}
