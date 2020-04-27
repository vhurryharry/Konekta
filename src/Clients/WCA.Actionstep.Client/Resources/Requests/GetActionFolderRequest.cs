using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class GetActionFolderRequest : IActionstepRequest
    {
        public ActionFolder ActionFolder { get; set; } = new ActionFolder();

        public string RelativeResourcePath =>
            string.IsNullOrEmpty(ActionFolder?.Links?.Action)
                ? "rest/actionfolders"
                : $"rest/actionfolders?action={ActionFolder?.Links?.Action}";


        public TokenSetQuery TokenSetQuery { get; set; }

        public HttpMethod HttpMethod => HttpMethod.Get;

        /// <summary>
        /// Not sure why this has content, because it's a Get request. Came across this
        /// during refactoring.
        /// </summary>
        public object JsonPayload => new { actionfolders = ActionFolder };

        public GetActionFolderRequest()
        {
        }

        public GetActionFolderRequest(TokenSetQuery tokenSetQuery, ActionFolder actionFolder)
        {
            TokenSetQuery = tokenSetQuery;
            ActionFolder = actionFolder;
        }
    }
}
