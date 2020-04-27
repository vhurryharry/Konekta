using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class SaveActionDocumentRequest : IActionstepRequest
    {
        public SaveActionDocumentRequest()
        {
        }

        public SaveActionDocumentRequest(TokenSetQuery tokenSetQuery, ActionDocument actionDocument)
        {
            TokenSetQuery = tokenSetQuery;
            ActionDocument = actionDocument;
        }

        public ActionDocument ActionDocument { get; set; }

        public HttpMethod HttpMethod => HttpMethod.Post;
        public string RelativeResourcePath => $"rest/actiondocuments";

        public TokenSetQuery TokenSetQuery { get; set; }

        public object JsonPayload => new { actiondocuments = ActionDocument };
    }
}
