using System;
using WCA.Domain.Actionstep;
using WCA.Domain.Models.Account;

namespace WCA.Domain.Models
{
    public class ReportSyncSignupSubmission
    {
        public int ReportSyncSignupSubmissionId { get; set; }
        public WCAUser CreatedBy { get; set; }
        public ActionstepOrg ActionstepOrg { get; set; }
        public string StripeId { get; set; }
        public string ServiceContactFirstname { get; set; }
        public string ServiceContactLastname { get; set; }
        public string ServiceContactEmail { get; set; }
        public string BillingContactFirstname { get; set; }
        public string BillingContactLastname { get; set; }
        public string BillingContactEmail { get; set; }
        public string BillingFrequency { get; set; }
        public string Company { get; set; }
        public string ABN { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public bool AcceptedTermsAndConditions { get; set; }
        public bool AcknowledgedFeesAndCharges { get; set; }
        public DateTime SubmittedDateTimeUtc { get; set; }
    }
}