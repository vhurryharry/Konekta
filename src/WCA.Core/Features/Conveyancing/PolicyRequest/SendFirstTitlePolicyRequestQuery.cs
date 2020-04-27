using System;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.FirstTitle.Client;

namespace WCA.Core.Features.Conveyancing.PolicyRequest
{
    public class SendFirstTitlePolicyRequestQuery : IAuthenticatedQuery<SendFirstTitlePolicyRequestResponse>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public FirstTitleCredential FirstTitleCredentials { get; set; }

        public int MatterId { get; set; }
        public string ActionstepOrg { get; set; }
        public RequestPolicyOptions RequestPolicyOptions { get; set; }
        public FTActionstepMatter ActionstepMatter { get; set; }
    }

    public class SendFirstTitlePolicyRequestResponse
    {
        public string PolicyNumber { get; set; }
        public FirstTitlePrice Price { get; set; }
        public FTAttachment[] AttachmentPaths { get; set; }
    }

    public class FTAttachment
    { 
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }

    public class FirstTitlePolicyRequestException: Exception
    {
        public FirstTitlePolicyRequestException() : base()
        {
        }

        public FirstTitlePolicyRequestException(string message) : base(message)
        {
        }

        public FirstTitlePolicyRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class FirstTitlePrice
    {
        public decimal Premium { get; set; }
        public decimal GSTOnPremium { get; set; }
        public decimal StampDuty { get; set; }
    }
}
