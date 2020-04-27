using System.Net.Http;

namespace WCA.PEXA.Client.Resources
{
    public class SubscriberInformation
    {
        public string SearchSubscriberId { get; set; }
        public string SubscriberName { get; set; }
    }

    public class SearchSubscriberRequestQuery: PEXARequestBase
    {
        public SearchSubscriberRequestQuery(SubscriberInformation subscriberInformation, string bearerToken)
        {
            BearerToken = bearerToken;
            SubscriberInformation = subscriberInformation;
        }

        public SubscriberInformation SubscriberInformation { get; set; }

        public override string Path
        {
            get
            {
                if(!string.IsNullOrEmpty(SubscriberInformation.SearchSubscriberId))
                    return "/v2/subscriber?searchSubscriberId=" + SubscriberInformation.SearchSubscriberId;

                if (!string.IsNullOrEmpty(SubscriberInformation.SubscriberName))
                    return "/v2/subscriber?subscriberName=" + SubscriberInformation.SubscriberName;

                return "/v2/subscriber";
            }
        }

        public override HttpMethod HttpMethod => HttpMethod.Get;

        public override HttpContent Content => null;

        public override int Version => 2;
    }
}
