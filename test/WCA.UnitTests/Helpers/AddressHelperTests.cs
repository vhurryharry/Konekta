using WCA.Core.Helpers;
using Xunit;

namespace WCA.UnitTests.Helpers
{
    public class AddressHelperTests
    {
        /// <summary>
        /// Parses the street number tests.
        /// </summary>
        /// <param name="addressLine1">The address line 1.</param>
        /// <param name="addressLine2">The address line 2.</param>
        /// <param name="expectedStreetNumber">
        ///     The expected street number. Note that this must match the
        ///     following pattern: /^\d+[a-zA-Z]?$/ to be compatible with InfoTrack.
        /// </param>
        /// <param name="expectedStreetName">Expected name of the street.</param>
        [Theory]
        [InlineData("1 Abc Street", null, "1", "Abc Street")]
        [InlineData("1 Abc Street", "Some place", "1", "Abc Street, Some place")]
        [InlineData("1A, 18 Albert Road", "", "18", "Albert Road")]
        [InlineData("1B Gawler Street", "", "1B", "Gawler Street")]
        [InlineData("1st Floor, 120 Hutt Street", "", "120", "Hutt Street")]
        [InlineData("30/23-25 Bunney Road", "", "25", "Bunney Road")]
        [InlineData("Adelaide Central Plaza", "", "", "Adelaide Central Plaza")]
        [InlineData("2/133 MOORE RESERVE", "", "133", "MOORE RESERVE")]
        [InlineData("2/133 MOORE SOMETHING RESERVE", "", "133", "MOORE SOMETHING RESERVE")]
        [InlineData("", "", "", "")]
        [InlineData(null, null, "", "")]
        public void ParseStreetNumberTests(
            string addressLine1,
            string addressLine2,
            string expectedStreetNumber,
            string expectedStreetName)
        {
            (string streetNumber, string streetName) =
                AddressHelper.ParseStreetNumber(addressLine1, addressLine2);
            Assert.Equal(expectedStreetNumber, streetNumber);
            Assert.Equal(expectedStreetName, streetName);
        }

        /// <summary>
        /// Parses the road information tests
        /// </summary>
        /// <param name="addressLine1">The address line 1.</param>
        /// <param name="addressLine2">The address line 2.</param>
        /// <param name="expectedRoadNumber">
        ///     The expected road number. Note that this must match the
        ///     following pattern: /^\d+[a-zA-Z]?$/ to be compatible with InfoTrack.
        /// </param>
        /// /// <param name="expectedRoadSuffixCode">
        ///     The expected Road Suffix Code
        /// </param>
        /// /// <param name="expectedRoadTypeCode">
        ///     The expected Road Type Code
        /// </param>
        /// <param name="expectedRoadName">Expected name of the road.</param>
        [Theory]
        [InlineData("1 Abc Street", null, "1", "", "ST", "Abc")]
        [InlineData("1 Abc Street", "North Street", "1", "N", "ST", "Abc Street")]
        [InlineData("1A, 18 Albert Road", "Inner Plaza", "18", "IN", "RD", "Albert Plaza")]
        [InlineData("1B ST Lower Gawler", "", "1B", "LR", "ST", "Gawler")]
        [InlineData("1st Floor, 120 Hutt Street", "", "120", "", "ST", "Hutt")]
        [InlineData("30/23-25 Bunney Road", "", "25", "", "RD", "Bunney")]
        [InlineData("30/23-25 ST Bunney", "North building", "25", "N", "ST", "Bunney building")]
        [InlineData("Adelaide Central Plaza", "", "", "CN", "PLZA", "Adelaide")]
        [InlineData("2/133 MOORE RESERVE", "", "133", "", "RES", "MOORE")]
        [InlineData("2/133 MOORE SOMETHING RESERVE", "", "133", "", "RES", "MOORE SOMETHING")]
        [InlineData("", "", "", "", "", "")]
        [InlineData(null, null, "", "", "", "")]
        public void ParseRoadInfoTests(
            string addressLine1,
            string addressLine2,
            string expectedRoadNumber,
            string expectedRoadSuffixCode,
            string expectedRoadTypeCode,
            string expectedRoadName)
        {
            (string roadNumber, string roadSuffixCode, string roadTypeCode, string streetName) =
                AddressHelper.ParseRoadInfo(addressLine1, addressLine2);
            Assert.Equal(expectedRoadNumber, roadNumber);
            Assert.Equal(expectedRoadSuffixCode, roadSuffixCode);
            Assert.Equal(expectedRoadTypeCode, roadTypeCode);
            Assert.Equal(expectedRoadName, streetName);
        }

    }
}
