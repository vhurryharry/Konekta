using System.Net.Http;

namespace WCA.PEXA.Client.Resources
{
    public class RetrieveSettlementAvailabilityParams
    {
        public RetrieveSettlementAvailabilityParams(
            string jurisdiction,
            string settlementDate,
            string subscriberId,
            string workspaceId = "")
        {
            Jurisdiction = jurisdiction;
            SettlementDate = settlementDate;
            SubscriberId = subscriberId;
            WorkspaceId = workspaceId;
        }

        public string Jurisdiction { get; set; }
        public string SettlementDate { get; set; }
        public string SubscriberId { get; set; }
        public string WorkspaceId { get; set; }
    }

    public class RetrieveSettlementAvailabilityQuery: PEXARequestBase
    {
        public RetrieveSettlementAvailabilityQuery(RetrieveSettlementAvailabilityParams retrieveSettlementAvailabilityParams, string bearerToken)
        {
            RetrieveSettlementAvailabilityParams = retrieveSettlementAvailabilityParams;
            BearerToken = bearerToken;
        }

        public RetrieveSettlementAvailabilityParams RetrieveSettlementAvailabilityParams { get; set; }

        public override string Path
        {
            get
            {
                if(RetrieveSettlementAvailabilityParams != null)
                {
                    var path = $"/v1/workspace/settlement?settlementDate={RetrieveSettlementAvailabilityParams.SettlementDate}&subscriberId={RetrieveSettlementAvailabilityParams.SubscriberId}";

                    if(!string.IsNullOrEmpty(RetrieveSettlementAvailabilityParams.Jurisdiction))
                    {
                        path = $"{path}&jurisdiction={RetrieveSettlementAvailabilityParams.Jurisdiction}";
                    } else
                    {
                        path = $"{path}&workspaceId={RetrieveSettlementAvailabilityParams.WorkspaceId}";
                    }

                    return path;
                }

                return "/v1/workspace/settlement";
            }
        }

        public override HttpMethod HttpMethod => HttpMethod.Get;

        public override HttpContent Content => null;

        public override int Version => 1;
    }
}
