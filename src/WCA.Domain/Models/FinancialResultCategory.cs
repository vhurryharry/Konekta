using System;
using System.Collections.Generic;
using System.Linq;

namespace WCA.Domain.Models
{
    public class FinancialResultCategory : IEquatable<FinancialResultCategory>
    {
        public FinancialResultCategory(
            string categoryTitle,
            FinancialResultLineItem[] lineItems)
        {
            Title = categoryTitle;
            LineItems = new List<FinancialResultLineItem>(lineItems);
            Total = Math.Round(LineItems.Sum(i => i.Value), 4);
        }

        public IEnumerable<FinancialResultLineItem> LineItems { get; }

        public string Title { get; }

        public decimal Total { get; }

        public override bool Equals(object other)
        {
            if (other == null) { return false; }

            var typedOther = other as FinancialResultCategory;
            if (other == null) { return false; }

            return Equals(typedOther);
        }

        public bool Equals(FinancialResultCategory other)
        {
            if (other == null) { return false; }

            return LineItems.SequenceEqual(other.LineItems) &&
                Title.Equals(other.Title, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return LineItems.GetHashCode() ^
                Title.GetHashCode(StringComparison.Ordinal);
        }
    }
}