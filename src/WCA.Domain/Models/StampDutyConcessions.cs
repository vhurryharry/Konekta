using System;

namespace WCA.Domain.Models
{
    public class StampDutyConcessions
    {
        public StampDutyConcessions(
            decimal firstHomeGrant,
            decimal other,
            decimal total)
        {
            FirstHomeGrant = firstHomeGrant;
            Other = other;
            Total = total;
        }

        public decimal FirstHomeGrant { get; }
        public decimal Other { get; }
        public decimal Total { get; }

        public override bool Equals(Object other)
        {
            if (other == null) { return false; }

            StampDutyConcessions typedOther = other as StampDutyConcessions;
            if (typedOther == null)
            { return false; }

            return Equals(typedOther);
        }

        public bool Equals(StampDutyConcessions other)
        {
            if (other == null) { return false; }

            return FirstHomeGrant == other.FirstHomeGrant &&
                Other == other.Other &&
                Total == other.Total;
        }

        public override int GetHashCode()
        {
            return FirstHomeGrant.GetHashCode() ^
                Other.GetHashCode() ^
                Total.GetHashCode();
        }
    }
}