using System;

namespace WCA.Domain.Models
{
    public abstract class RangeBase<TRangeType> : IComparable<RangeBase<TRangeType>>, IEquatable<TRangeType>, IRange<TRangeType>
        where TRangeType : IRange<TRangeType>
    {
        /// <summary>
        /// Whether intersects can be touching or not. For example, if set to false then the following
        /// ranges are deemed to be intersecting: 10-5000, 5000-10000. If set to true, then the example
        /// would be deemed not to be intersecting.
        /// </summary>
        public bool AllowTouchingIntersects { get; protected set; } = true;

        public decimal From { get; protected set; }
        public decimal To { get; protected set; }

        public RangeBase(decimal from, decimal to)
        {
            if (from > to)
            {
                throw new ArgumentOutOfRangeException(nameof(to), "The parameter 'to' must be greater than or equal to 'from'.");
            }

            From = from;
            To = to;
        }

        public RangeBase(decimal from, decimal to, bool allowTouchingIntersects) : this(from, to)
        {
            AllowTouchingIntersects = allowTouchingIntersects;
        }

        public int CompareTo(RangeBase<TRangeType> other)
        {
            return From.CompareTo(other?.From);
        }

        public abstract bool Equals(TRangeType other);

        public bool Intersects(TRangeType range)
        {
            if (AllowTouchingIntersects)
            {
                if (From < range.To)
                {
                    return (To > range.From);
                }
                else if (To > range.From)
                {
                    return (From < range.To);
                }
            }
            else
            {
                if (From <= range.To)
                {
                    return (To >= range.From);
                }
                else if (To >= range.From)
                {
                    return (From <= range.To);
                }
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (From, To, AllowTouchingIntersects).GetHashCode();
        }

        public static bool operator ==(RangeBase<TRangeType> left, RangeBase<TRangeType> right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(RangeBase<TRangeType> left, RangeBase<TRangeType> right)
        {
            return !(left == right);
        }

        public static bool operator <(RangeBase<TRangeType> left, RangeBase<TRangeType> right)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(RangeBase<TRangeType> left, RangeBase<TRangeType> right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >(RangeBase<TRangeType> left, RangeBase<TRangeType> right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(RangeBase<TRangeType> left, RangeBase<TRangeType> right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
        }
    }
}