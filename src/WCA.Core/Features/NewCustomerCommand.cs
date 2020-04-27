using System;
using WCA.Domain.CQRS;

namespace WCA.Core.Features
{
    public class NewCustomerCommand : ICommand<NewCustomerCreated>
    {
        public string StripeApiSecret { get; set; }
        public string PaymentGatewayToken { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string ConveyancingApp { get; set; }
        public string ABN { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string PromoCode { get; set; }
        public string OrgKey { get; set; }
        public bool AcceptedTermsAndConditions { get; set; }
        public DateTime SubmittedDateTimeUtc { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}