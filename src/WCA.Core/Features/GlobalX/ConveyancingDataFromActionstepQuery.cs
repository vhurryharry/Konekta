using System;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.GlobalX
{
    public class ConveyancingDataFromActionstepQuery : IAuthenticatedQuery<RequestPropertyInformationFromActionstepResponse>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public int MatterId { get; set; }
        public string OrgKey { get; set; }
        public string EntryPoint { get; set; }
        public bool Embed { get; set; }
    }

    public class RequestPropertyInformationFromActionstepResponse
    {
        public string Matter { get; set; }
        public string Version { get; set; }
        public Uri GXUri { get; set; }
    }
}
