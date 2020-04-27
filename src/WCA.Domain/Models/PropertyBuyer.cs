namespace WCA.Domain.Models
{
    public class PropertyBuyer
    {
        public PropertyBuyer()
        { }

        public PropertyBuyer(
            int buyerNumber,
            IntendedPropertyUse intendedUse,
            bool firstHomeBuyer,
            bool isForeignBuyer,
            Fraction shares)
        {
            BuyerNumber = buyerNumber;
            IntendedUse = intendedUse;
            FirstHomeBuyer = firstHomeBuyer;
            IsForeignBuyer = isForeignBuyer;
            Shares = shares;
        }

        public int BuyerNumber { get; set; }
        public bool FirstHomeBuyer { get; set; }
        public IntendedPropertyUse IntendedUse { get; set; }
        public bool IsForeignBuyer { get; set; }
        public Fraction Shares { get; set; }
    }
}