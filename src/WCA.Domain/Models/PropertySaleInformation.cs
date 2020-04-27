using System;
using System.Collections.Generic;
using System.Linq;

namespace WCA.Domain.Models
{
    public enum IntendedPropertyUse
    {
        PrimaryResidence = 1,
        Investment
    };

    public enum PropertyType
    {
        EstablishedHome = 1,
        NewHome,
        VacantLand
    };

    public enum State
    {
        ACT = 1,
        NSW,
        NT,
        QLD,
        SA,
        TAS,
        VIC,
        WA,
    };

    public class PropertySaleInformation
    {
        private List<PropertyBuyer> _buyers;

        public PropertySaleInformation()
        {
            _buyers = new List<PropertyBuyer>();
        }

        public PropertySaleInformation(
            decimal purchasePrice,
            State state,
            PropertyType propertyType,
            PropertyBuyer[] buyers)
        {
            PurchasePrice = purchasePrice;
            State = state;
            PropertyType = propertyType;
            _buyers = new List<PropertyBuyer>();

            if (buyers != null)
            {
                foreach (PropertyBuyer buyer in buyers)
                {
                    AddBuyer(buyer);
                }
            }
        }

        public IEnumerable<PropertyBuyer> Buyers
        {
            get
            {
                return _buyers.AsReadOnly();
            }
        }

        public PropertyType PropertyType { get; set; }

        public decimal PurchasePrice { get; set; }

        public State State { get; set; }

        public void AddBuyer(PropertyBuyer buyer)
        {
            if (_buyers.Any(b => b.BuyerNumber == buyer.BuyerNumber))
            {
                throw new ArgumentException($"A buyer already exists with the specified BuyerNumber. Buyer number '{buyer?.BuyerNumber}'.", nameof(buyer));
            }

            _buyers.Add(buyer);
        }
    }
}