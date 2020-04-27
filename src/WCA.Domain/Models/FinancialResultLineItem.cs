using System;

namespace WCA.Domain.Models
{
    public class FinancialResultLineItem : IEquatable<FinancialResultLineItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialResultLineItem"/> class.
        /// </summary>
        /// <param name="lineItemTitle">The line item title.</param>
        /// <param name="value">The value for the line item. Value will be rounded to the nearest four decimal places.</param>
        public FinancialResultLineItem(string lineItemTitle, decimal value)
        {
            LineItemTitle = lineItemTitle;
            Value = Math.Round(value, 4);
        }

        public string LineItemTitle { get; }

        public decimal Value { get; }

        public override bool Equals(object other)
        {
            if (other == null) { return false; }

            FinancialResultLineItem typedOther = other as FinancialResultLineItem;
            if (typedOther == null) { return false; }

            return Equals(typedOther);
        }

        public bool Equals(FinancialResultLineItem other)
        {
            if (other == null) { return false; }

            return LineItemTitle.Equals(other.LineItemTitle, StringComparison.InvariantCultureIgnoreCase) &&
                Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return LineItemTitle.GetHashCode(StringComparison.Ordinal) ^
                Value.GetHashCode();
        }
    }
}