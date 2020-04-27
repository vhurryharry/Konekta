using System.Net.Http;

namespace WCA.PEXA.Client.Resources
{
    public class RetrieveWorkspaceSummaryParameters
    {
        public RetrieveWorkspaceSummaryParameters(
            string workspaceId,
            string lastEventId = "",
            string subscriberId = "",
            string subscriberRole = "",
            string subset = "")
        {
            WorkspaceId = workspaceId;
            LastEventId = lastEventId;
            SubscriberId = subscriberId;
            SubscriberRole = subscriberRole;
            Subset = subset;
        }

        public string LastEventId { get; set; }
        public string SubscriberId { get; set; }
        public string SubscriberRole { get; set; }
        public string Subset { get; set; }
        public string WorkspaceId { get; set; }
    }

    public class RetrieveWorkspaceSummaryRequestQuery : PEXARequestBase
    {
        public RetrieveWorkspaceSummaryRequestQuery(RetrieveWorkspaceSummaryParameters retrieveWorkspaceSummaryParameters, string bearerToken)
        {
            RetrieveWorkspaceSummaryParameters = retrieveWorkspaceSummaryParameters;
            BearerToken = bearerToken;
        }

        public RetrieveWorkspaceSummaryParameters RetrieveWorkspaceSummaryParameters { get; set; }

        public override string Path
        {
            get
            {
                if (RetrieveWorkspaceSummaryParameters != null)
                {
                    var path = $"/v1/workspace?workspaceId={RetrieveWorkspaceSummaryParameters.WorkspaceId}";
                    if (!string.IsNullOrWhiteSpace(RetrieveWorkspaceSummaryParameters.LastEventId))
                        path += $"&lastEventId={RetrieveWorkspaceSummaryParameters.LastEventId}";

                    if (!string.IsNullOrWhiteSpace(RetrieveWorkspaceSummaryParameters.SubscriberId))
                        path += $"&subscriberId ={RetrieveWorkspaceSummaryParameters.SubscriberId}";

                    if (!string.IsNullOrWhiteSpace(RetrieveWorkspaceSummaryParameters.SubscriberRole))
                        path += $"&subscriberRole={RetrieveWorkspaceSummaryParameters.SubscriberRole}";

                    if (!string.IsNullOrWhiteSpace(RetrieveWorkspaceSummaryParameters.Subset))
                        path += $"&subset={RetrieveWorkspaceSummaryParameters.Subset}";

                    return path;
                }

                return "/v1/workspace";
            }
        }

        public override HttpMethod HttpMethod => HttpMethod.Get;

        public override HttpContent Content => null;

        public override int Version => 1;
    }
}
