using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using WCA.Domain.Models;

namespace WCA.Web.Areas.API.StampDutyCalculator
{
    public class PropertyBuyerViewModel
    {
        private const string PARAM_PURCHASE_FRACTION = "purchaseFraction";

        public PropertyBuyerViewModel(
            int buyerNumber,
            IntendedPropertyUse intendedUse,
            bool firstHomeBuyer,
            bool isForeignBuyer,
            string purchaseFraction)
        {
            try
            {
                PurchaseFractionAsFraction = new Fraction(purchaseFraction);
                PurchaseFraction = purchaseFraction;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid purchase fraction specified. The purchase fraction must either be a fraction such as 1/2, 3/8, or the number 1.", PARAM_PURCHASE_FRACTION, ex);
            }

            if (PurchaseFractionAsFraction.Numerator > PurchaseFractionAsFraction.Denominator)
            {
                throw new ArgumentException("The purchase fraction cannot be greater than 1.", PARAM_PURCHASE_FRACTION);
            }

            BuyerNumber = buyerNumber;
            IntendedUse = intendedUse;
            FirstHomeBuyer = firstHomeBuyer;
            IsForeignBuyer = isForeignBuyer;
        }

        public int BuyerNumber { get; }

        [Required]
        [Display(Name = "Is first home buyer")]
        public bool FirstHomeBuyer { get; }

        [Required]
        [Display(Name = "Intended use")]
        public IntendedPropertyUse IntendedUse { get; }

        [Required]
        [Display(Name = "Is foreign buyer")]
        public bool IsForeignBuyer { get; }

        [Required]
        [Display(Name = "Purchase Fraction", Description = "The fraction of the property that this buyer is purchasing.")]
        public string PurchaseFraction { get; }

        [JsonIgnore]
        public Fraction PurchaseFractionAsFraction { get; }
    }
}