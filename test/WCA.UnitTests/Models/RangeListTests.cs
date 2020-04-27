using System;
using WCA.Domain.Models;
using Xunit;

namespace WCA.UnitTests.Models
{
    public class RangeListTests
    {
        [Fact]
        public void RangeListCanBeCreatedWithNonOverlappingRanges()
        {
            RangeList<SimpleRange> list = new RangeList<SimpleRange>(new[] {
                    new SimpleRange(0,10),
                    new SimpleRange(10,15)
            });

            Assert.Equal(2, list.Count);
        }

        [Fact]
        public void RangeListDoesntAllowInitializationWithOverlappingRanges()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                RangeList<SimpleRange> list = new RangeList<SimpleRange>(new[] {
                    new SimpleRange(0, 10),
                    new SimpleRange(5, 15)
                });
            });
        }

        [Fact]
        public void RangeListDoesntAllowOverlappingRangeToBeAdded()
        {
            RangeList<SimpleRange> list = new RangeList<SimpleRange>(new[] {
                new SimpleRange(0, 10)
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                list.Add(new SimpleRange(1, 15));
            });
        }

        [Theory]
        [InlineData(10, 20, 20, 30, false)]
        [InlineData(10, 20, 5, 10, false)]
        [InlineData(10, 25, 20, 30, true)]
        [InlineData(10, 20, 5, 11, true)]
        public void RangeWithAllowedTouchingIntersect(
            decimal from1,
            decimal to1,
            decimal from2,
            decimal to2,
            bool shouldIntersect)
        {
            SimpleRange range1 = new SimpleRange(from1, to1, true);
            SimpleRange range2 = new SimpleRange(from2, to2, true);
            Assert.Equal(shouldIntersect, range1.Intersects(range2));
        }

        [Theory]
        [InlineData(10, 20, 20, 30, true)]
        [InlineData(10, 20, 5, 10, true)]
        [InlineData(10, 25, 20, 30, true)]
        [InlineData(10, 20, 5, 11, true)]
        [InlineData(10, 19.99, 20, 30, false)]
        public void RangeWithDisallowedTouchingIntersect(
            decimal from1,
            decimal to1,
            decimal from2,
            decimal to2,
            bool shouldIntersect)
        {
            SimpleRange range1 = new SimpleRange(from1, to1, false);
            SimpleRange range2 = new SimpleRange(from2, to2, false);
            Assert.Equal(shouldIntersect, range1.Intersects(range2));
        }
    }
}