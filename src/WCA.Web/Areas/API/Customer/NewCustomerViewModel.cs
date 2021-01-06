using System.ComponentModel.DataAnnotations;
using WCA.Domain.Validators;

namespace WCA.Web.Areas.API.Customer
{
    public class NewCustomerViewModel
    {
        [Display(Name = "ABN")]
        [ABN(AllowNullOrEmpty = true)]
        public string ABN { get; set; }

        [Required]
        [Display(Name = "Company")]
        public string Company { get; set; }

        [Required]
        public string ConveyancingApp { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string PromoCode { get; set; }

        [Required(ErrorMessage = "Please select a valid Actionstep organisation")]
        public string OrgKey { get; set; }

        public string PaymentGatewayToken { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        public bool acceptedTermsAndConditions { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }
    }
}