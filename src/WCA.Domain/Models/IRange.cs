namespace WCA.Domain.Models
{
    public interface IRange<TRangeType>
        where TRangeType : IRange<TRangeType>
    {
        decimal From { get; }
#pragma warning disable CA1716 // Identifiers should not match keywords
        decimal To { get; }
#pragma warning restore CA1716 // Identifiers should not match keywords

        bool Intersects(TRangeType range);
    }
}