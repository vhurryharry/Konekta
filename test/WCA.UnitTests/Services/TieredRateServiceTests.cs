using System;
using System.Collections.Generic;
using WCA.Core.Services;
using Xunit;

namespace WCA.UnitTests.Services
{
    public class TieredRateServiceFixture
    {
        public TieredRateServiceFixture()
        {
            Tiers = new[] {
                new TieredRateTier(0, 1000, 0, 0),
                new TieredRateTier(1000, 5000, 100, 0),
                new TieredRateTier(5000, 75000, 100, 1.5M),
                new TieredRateTier(75000, 540000, 100, 3.5M),
                new TieredRateTier(540000, 1000000, 100, 4.5M),
                new TieredRateTier(1000000, decimal.MaxValue, 100, 5.75M)
            };

            TieredRateService = new TieredRateService(Tiers);
        }

        public TieredRateService TieredRateService { get; private set; }
        public IEnumerable<TieredRateTier> Tiers { get; private set; }
    }

    public class TieredRateServiceTests : IClassFixture<TieredRateServiceFixture>
    {
        private TieredRateServiceFixture fixture;

        public TieredRateServiceTests(TieredRateServiceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Theory]
        [InlineData(4999, 0)]
        [InlineData(5000, 0)]
        [InlineData(5001, 1.5)]
        [InlineData(5201, 4.5)]
        [InlineData(75000, 1050)]
        [InlineData(540000, 17325)]
        [InlineData(540001, 17329.5)]
        [InlineData(1000000, 38025)]
        [InlineData(1000001, 38030.75)]
        [InlineData(1000100, 38030.75)]
        public void CalculateTieredRateTest(decimal amount, decimal expectedRate)
        {
            Assert.Equal(expectedRate, fixture.TieredRateService.CalculateTieredRate(amount));
        }

        [Fact]
        public void TieredRateServiceThrowsOnOverlappingRange()
        {
            TieredRateTier[] tiersWithOverlappingRanges = new[] {
                new TieredRateTier(0, 5000, 100, 0),
                new TieredRateTier(5000, 75001, 100, 1.5M),
                new TieredRateTier(75000, 540000, 100, 3.5M),
                new TieredRateTier(540000, 1000000, 100, 4.5M),
                new TieredRateTier(1000000, decimal.MaxValue, 100, 5.75M)
            };

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                TieredRateService service = new TieredRateService(tiersWithOverlappingRanges);
            });
        }

        [Fact]
        public void TieredRateServiceThrowsOnOverlappingRange2()
        {
            TieredRateTier[] tiersWithOverlappingRanges = new[] {
                new TieredRateTier(5000, 75001, 100, 1.5M),
                new TieredRateTier(5000, 75001, 100, 1.5M)
            };

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                TieredRateService service = new TieredRateService(tiersWithOverlappingRanges);
            });
        }

        [Fact]
        public void TieredRateTierHighEndCanBeEqualToLowEnd()
        {
            TieredRateTier testTier = new TieredRateTier(1, 1, 0, 0);

            Assert.NotNull(testTier);
        }

        [Fact]
        public void TieredRateTierHighEndMustBeGreaterThanLowEnd()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                TieredRateTier testTier = new TieredRateTier(1, 0, 0, 0);
            });
        }
    }
}