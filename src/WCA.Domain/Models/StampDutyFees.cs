using System;

namespace WCA.Domain.Models
{
    public class StampDutyFees
    {
        public StampDutyFees(
            decimal mortgageFee,
            decimal transferFee,
            decimal stampDutyFee,
            decimal foreignBuyersDuty,
            decimal totalFees)
        {
            MortgageFee = mortgageFee;
            TransferFee = transferFee;
            StampDutyFee = stampDutyFee;
            ForeignBuyersDuty = foreignBuyersDuty;
            TotalFees = totalFees;
        }

        public decimal ForeignBuyersDuty { get; }
        public decimal MortgageFee { get; }
        public decimal StampDutyFee { get; }
        public decimal TotalFees { get; }
        public decimal TransferFee { get; }

        public override bool Equals(Object other)
        {
            if (other == null) { return false; }

            StampDutyFees typedOther = other as StampDutyFees;
            if (typedOther == null)
            { return false; }

            return Equals(typedOther);
        }

        public bool Equals(StampDutyFees other)
        {
            if (other == null) { return false; }

            return ForeignBuyersDuty == other.ForeignBuyersDuty &&
                MortgageFee == other.MortgageFee &&
                StampDutyFee == other.StampDutyFee &&
                TotalFees == other.TotalFees &&
                TransferFee == other.TransferFee;
        }

        public override int GetHashCode()
        {
            return ForeignBuyersDuty.GetHashCode() ^
                MortgageFee.GetHashCode() ^
                StampDutyFee.GetHashCode() ^
                TotalFees.GetHashCode() ^
                TransferFee.GetHashCode();
        }
    }
}