namespace WCA.Core.Features.Conveyancing.StampDutyCalculator
{
    public class StampDutyCalculatorInfo
    {
        public decimal PurchasePrice { get; }
        public string PropertyType { get; }

        public StampDutyCalculatorInfo(
            decimal purchasePrice,
            string propertyType)
        {
            PurchasePrice = purchasePrice;
            PropertyType = propertyType;
        }
    }
}
