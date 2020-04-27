using System;
using System.Collections.Generic;
using System.Linq;

namespace WCA.Domain.Models
{
    public class FinancialResults : IEquatable<FinancialResults>
    {
        public IEnumerable<FinancialResultCategory> Categories { get; }

        public FinancialResults(FinancialResultCategory[] categories)
        {
            Categories = new List<FinancialResultCategory>(categories);
        }

        public override bool Equals(object other)
        {
            if (other == null) { return false; }

            var typedOther = other as FinancialResults;
            if (typedOther == null) { return false; }

            return Equals(typedOther);
        }

        public bool Equals(FinancialResults other)
        {
            if (other == null) { return false; }

            return Categories.SequenceEqual(other.Categories);
        }

        public override int GetHashCode()
        {
            return Categories.GetHashCode();
        }
    }
}