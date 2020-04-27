namespace WCA.Domain.Models
{
    /// <summary>
    /// A simple range implementation, primarily intended for testing RangeBase.
    /// </summary>
    /// <seealso cref="WCA.Core.Models.RangeBase{WCA.Core.Models.SimpleRange}" />
    public class SimpleRange : RangeBase<SimpleRange>
    {
        public SimpleRange(decimal from, decimal to, bool allowTouchingIntersects = true) : base(from, to, allowTouchingIntersects)
        {
        }

        public override bool Equals(SimpleRange other)
        {
            if (other == null)
            {
                return false;
            }

            return From == other.From && To == other.To;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SimpleRange);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}