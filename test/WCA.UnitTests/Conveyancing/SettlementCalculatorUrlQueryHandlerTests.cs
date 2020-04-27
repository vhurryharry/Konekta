using WCA.Core.Features.Conveyancing.SettlementCalculator;
using Xunit;

namespace WCA.UnitTests.Conveyancing
{
    public class SettlementCalculatorUrlQueryHandlerTests
    {
        [Fact]
        public void DateParamEncodedCorrectly()
        {
            // Act
            var result = SettlementCalculatorUrlQueryHandler.ParseAndFormatDateParam("param", "2019-12-03");

            // Assert
            Assert.Equal("param=03%20December%202019", result);
        }

        [Fact]
        public void DateEmptyStringIfMissingParam()
        {
            // Act
            var result = SettlementCalculatorUrlQueryHandler.ParseAndFormatDateParam("", "2019-12-03");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void DateEmptyStringIfInvalidDate()
        {
            // Act
            var result = SettlementCalculatorUrlQueryHandler.ParseAndFormatDateParam("param", "abc123");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void CurrencyParamEncodedCorrectly()
        {
            // Act
            var result = SettlementCalculatorUrlQueryHandler.ParseAndFormatCurrencyParam("param", "725000");

            // Assert
            Assert.Equal("param=$725,000.00", result);
        }

        [Fact]
        public void CurrencyEmptyStringIfMissingParam()
        {
            // Act
            var result = SettlementCalculatorUrlQueryHandler.ParseAndFormatCurrencyParam("", "725000");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void CurrencyEmptyStringIfInvalidDate()
        {
            // Act
            var result = SettlementCalculatorUrlQueryHandler.ParseAndFormatCurrencyParam("param", "aaa");

            // Assert
            Assert.Equal("", result);
        }
    }
}
