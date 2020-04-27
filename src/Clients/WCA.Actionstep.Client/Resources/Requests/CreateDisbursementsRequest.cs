using System.Collections.Generic;
using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class CreateDisbursementsRequest : IActionstepRequest
    {
        public List<Disbursement> Disbursements { get; } = new List<Disbursement>();

        public string RelativeResourcePath
        {
            get => $"rest/disbursements";
        }

        public HttpMethod HttpMethod => HttpMethod.Post;

        public TokenSetQuery TokenSetQuery { get; set; }

        public object JsonPayload => new { disbursements = Disbursements };
    }
}
