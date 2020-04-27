using System.Net.Http;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;

namespace WCA.Actionstep.Client
{
    public class ActionstepHttpRequestMessage : HttpRequestMessage
    {
        public ActionstepHttpRequestMessage(IActionstepRequest actionstepRequest, IActionstepService actionstepService, TokenSet initialTokenSet)
        {
            // We set a dummy RequestUri because the RequestUri will be
            // modified by ActionstepAuthDelegatingHandler.
            RequestUri = new System.Uri("https://0.0.0.0");

            ActionstepRequest = actionstepRequest;
            ActionstepService = actionstepService;
            TokenSet = initialTokenSet;
        }

        public IActionstepRequest ActionstepRequest { get; }
        public IActionstepService ActionstepService{ get; }

        /// <summary>
        /// Must be settable in the event of a refresh.
        /// </summary>
        public TokenSet TokenSet { get; set; }
    }
}
