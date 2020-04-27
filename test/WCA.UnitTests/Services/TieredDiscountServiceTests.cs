using WCA.Core.Services;
using Xunit;

namespace WCA.UnitTests.Services
{
    public class TieredDiscountServiceTests
    {
        [Theory]
        [InlineData(4999, 10)]
        [InlineData(5000, 100)]
        [InlineData(5001, 100)]
        [InlineData(999999, 200)]
        public void TieredRateServiceTestWithMaximum(decimal amount, decimal expectedDiscount)
        {
            TieredDiscountTier[] tiers = new[] {
                new TieredDiscountTier(0, 4999.99M, 10),
                new TieredDiscountTier(5000, 7499.99M, 100),
                new TieredDiscountTier(75000, 539999.99M, 200),
                new TieredDiscountTier(540000, decimal.MaxValue, 200)
            };

            TieredDiscountService service = new TieredDiscountService(tiers);
            decimal discount = service.CalculateDiscountRate(amount);
            Assert.Equal(expectedDiscount, discount);
        }

        [Theory]
        [InlineData(4999, 10)]
        [InlineData(5000, 100)]
        [InlineData(5001, 100)]
        [InlineData(999999, 0)]
        public void TieredRateServiceTestWithNoMaximum(decimal amount, decimal expectedDiscount)
        {
            TieredDiscountTier[] tiers = new[] {
                new TieredDiscountTier(0, 4999.99M, 10),
                new TieredDiscountTier(5000, 74999.99M, 100),
                new TieredDiscountTier(75000, 540000, 200)
            };

            TieredDiscountService service = new TieredDiscountService(tiers);
            decimal discount = service.CalculateDiscountRate(amount);
            Assert.Equal(expectedDiscount, discount);
        }
    }
}