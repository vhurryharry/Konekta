using System.ComponentModel.DataAnnotations;
using System.Linq;
using WCA.Domain.Models;

namespace WCA.Web.Areas.API.StampDutyCalculator
{
    public class PropertySaleInformationViewModel
    {
        public PropertySaleInformationViewModel()
        {
        }

        public PropertySaleInformationViewModel(
            decimal purchasePrice,
            State state,
            PropertyType propertyType)
        {
            PurchasePrice = purchasePrice;
            State = state;
            PropertyType = propertyType;
        }

        [Required]
        [Display(Name = "Buyers")]
#pragma warning disable CA1819 // Properties should not return arrays: This is a DTO
        public PropertyBuyerViewModel[] Buyers { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        [Required]
        [Display(Name = "Property type")]
        public PropertyType PropertyType { get; set; }

        [Required]
        [Display(Name = "Purchase price")]
        public decimal PurchasePrice { get; set; }

        [Required]
        [Display(Name = "State")]
        public State State { get; set; }

        public PropertySaleInformation ToDomainModel()
        {
            return new PropertySaleInformation(PurchasePrice,
                State,
                PropertyType,
                Buyers.Select(b =>
                    new PropertyBuyer(b.BuyerNumber, b.IntendedUse, b.FirstHomeBuyer, b.IsForeignBuyer, b.PurchaseFraction)).ToArray()
                );
        }
    }
}