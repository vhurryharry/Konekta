
using System.Collections.Generic;
using WCA.Domain.Models;

namespace WCA.Core.Services
{
    public class TieredDiscountService
    {
        private RangeList<TieredDiscountTier> instanceTiers;

        public TieredDiscountService(IEnumerable<TieredDiscountTier> tiers)
        {
            instanceTiers = new RangeList<TieredDiscountTier>(tiers);
        }

        /// <summary>
        /// Calculates a standard tiered rate based on the given tier information.
        /// </summary>
        /// <param name="amount">The base amount.</param>
        /// <returns>The discount, if the amount matches one of the discount tiers.</returns>
        public decimal CalculateDiscountRate(decimal amount)
        {
            foreach (TieredDiscountTier tier in instanceTiers)
            {
                if (amount >= tier.From && amount <= tier.To)
                {
                    return tier.Discount;
                }
            }

            // Default to 0 if no discount rate is found
            return 0M;
        }
    }

    /// <summary>
    /// CompareTo performs a comparison on the <see cref="RangeBase{TRangeType}.From"/> property.
    /// </summary>
    /// <seealso cref="WCA.Core.Models.RangeBase{WCA.Core.Services.TieredDiscountTier}" />
    /// <seealso cref="System.IEquatable{WCA.Core.Services.TieredDiscountTier}" />
    /// <seealso cref="System.IComparable{WCA.Core.Services.TieredDiscountTier}" />
    public class TieredDiscountTier : RangeBase<TieredDiscountTier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TieredDiscountTier"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="discount">The discount.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">to - The parameter 'to' must be greater than or equal to 'from'.</exception>
        public TieredDiscountTier(
            decimal from,
            decimal to,
            decimal discount) : base(from, to)
        {
            Discount = discount;

            // Don't allow intersects for this type of range.
            AllowTouchingIntersects = false;
        }

        /// <summary>
        /// Gets the discount.
        /// </summary>
        /// <value>
        /// The discount.
        /// </value>
        public decimal Discount { get; private set; }

        public override bool Equals(TieredDiscountTier other)
        {
            if (From == other?.From &&
                To == other.To &&
                Discount == other.Discount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TieredDiscountTier);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}