using System;
using System.Collections.Generic;
using System.Linq;
using WCA.Domain.Models;

namespace WCA.Core.Services
{
    public class TieredRateService
    {
        private RangeList<TieredRateTier> instanceTiers;

        public TieredRateService(IEnumerable<TieredRateTier> tiers)
        {
            instanceTiers = new RangeList<TieredRateTier>(tiers);

            // Rely on TieredRateTier CompareTo, which uses From.
            // Internal calculations rely on this being sorted by From ascending order.
            instanceTiers.Sort();
        }

        /// <summary>
        /// Calculates a standard tiered rate based on the given tier information.
        /// </summary>
        /// <param name="amount">The base amount.</param>
        /// <returns></returns>
        public decimal CalculateTieredRate(decimal amount)
        {
            decimal rate = 0;

            foreach (TieredRateTier tier in instanceTiers)
            {
                if (amount > tier.From && tier.PartSize > 0)
                {
                    decimal amountToCalculateForThisTier = (amount > tier.To)
                        ? tier.To - tier.From
                        : amount - tier.From;

                    rate += (Math.Ceiling(amountToCalculateForThisTier / tier.PartSize)) * tier.PartMultiplier;
                }

                // Since tiers are ordered, if our amount is more than the highest high end
                // we can stop as it won't match any further tiers.
                if (amount <= tier.To)
                {
                    break;
                }
            }

            return rate;
        }

        private TieredRateTier FindTierForAmount(decimal amount)
        {
            return instanceTiers.Where(t => amount > t.From && amount <= t.To).Single();
        }
    }

    /// <summary>
    /// Tiered Rate Tier for calculating tiered rates.
    /// </summary>
    /// <seealso cref="WCA.Core.Models.RangeBase{WCA.Core.Services.TieredRateTier}" />
    public class TieredRateTier : RangeBase<TieredRateTier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TieredRateTier"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="partSize">Size of the part.</param>
        /// <param name="partMultiplier">The part multiplier.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">to - The parameter 'to' must be greater than or equal to 'from'.</exception>
        public TieredRateTier(
            decimal from,
            decimal to,
            decimal partSize,
            decimal partMultiplier) : base(from, to)
        {
            PartSize = partSize;
            PartMultiplier = partMultiplier;
        }

        /// <summary>
        /// Gets the part multiplier.
        /// E.g. the cost for each $100 (partSize) above $5000 (from).
        /// </summary>
        /// <value>
        /// The part multiplier.
        /// </value>
        public decimal PartMultiplier { get; private set; }

        /// <summary>
        /// Gets the size of the part.
        /// Size of the part. E.g. every $100 or part thereof above $5000 (from) and below $75000 (to).
        /// </summary>
        /// <value>
        /// The size of the part.
        /// </value>
        public decimal PartSize { get; private set; }

        public override bool Equals(TieredRateTier other)
        {
            if (From == other?.From &&
                To == other.To &&
                PartSize == other.PartSize &&
                PartMultiplier == other.PartMultiplier)
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
            return Equals(obj as TieredRateTier);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}