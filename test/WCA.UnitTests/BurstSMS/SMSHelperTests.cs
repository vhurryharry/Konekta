using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WCA.AzureFunctions.EmailToSMS;
using Xunit;

namespace WCA.UnitTests.BurstSMS
{
    public class SMSHelperTests
    {
        [Theory]
        [InlineData("+61 444 111222", "+61444111222")]
        [InlineData("+61 (444) 111222", "+61444111222")]
        [InlineData("+61 444-111222", "+61444111222")]
        public void MobileNumberTests(string uncleanMobileNumber, string expectedResult)
        {
            var result = SMSHelper.CleanMobileNumber(uncleanMobileNumber);

            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// Must set to a valid Date Time accepted by Burst SMS:
        /// Field "send_at" is not a valid date time format (YYYY-MM-DD HH:MM:SS) or (YYYY-MM-DD).
        /// </summary>
        /// <param name="sendAt"></param>
        /// <param name="timeZoneId"></param>
        /// <param name="expectedResult"></param>
        [Theory]
        // General scenarios
        [InlineData("2020-01-01T11:00:00", "Australia/Sydney", "2020-01-01 00:00:00")]
        [InlineData("2020-07-01T10:00:00", "Australia/Sydney", "2020-07-01 00:00:00")]
        [InlineData("2020-01-01T00:00:00Z", "Australia/Sydney", "2020-01-01 00:00:00")]      // Contains an offset (Zulu), so ignore time zone ID and use the offset
        [InlineData("2020-01-01T01:00:00+01:00", "Australia/Sydney", "2020-01-01 00:00:00")] // Contains an offset, so ignore time zone ID and use the offset
        [InlineData("2020-01-01T11:00", "Australia/Sydney", "2020-01-01 00:00:00")]          // Assume 00 seconds
        [InlineData("2020-07-01T10:00", "Australia/Sydney", "2020-07-01 00:00:00")]          // Assume 00 seconds

        // Fall-back to default time zone
        [InlineData("2020-01-01T11:00:00", "", "2020-01-01 00:00:00")]                       // If no time zone, default to Sydney (DST)
        [InlineData("2020-07-01T10:00:00", "", "2020-07-01 00:00:00")]                       // If no time zone, default to Sydney
        [InlineData("2020-01-01T11:00:00", "Invalid Time Zone", "2020-01-01 00:00:00")]      // If invalid time zone, default to Sydney (DST)
        [InlineData("2020-07-01T10:00:00", "Invalid Time Zone", "2020-07-01 00:00:00")]      // If invalid time zone, default to Sydney

        // Invalid / pass throughs
        [InlineData("Invalid Send At Value", "Australia/Sydney", "Invalid Send At Value")]   // Pass through and let destination handle it
        [InlineData("", "Australia/Sydney", "")]

        // State based lookups
        [InlineData("2020-01-01T11:00:00", "ACT", "2020-01-01 00:00:00")]
        [InlineData("2020-01-01T11:00:00", "NSW", "2020-01-01 00:00:00")]
        [InlineData("2020-01-01T09:30:00", "NT",  "2020-01-01 00:00:00")]
        [InlineData("2020-01-01T10:00:00", "QLD", "2020-01-01 00:00:00")]
        [InlineData("2020-01-01T10:30:00", "SA",  "2020-01-01 00:00:00")]
        [InlineData("2020-01-01T11:00:00", "TAS", "2020-01-01 00:00:00")]
        [InlineData("2020-01-01T11:00:00", "VIC", "2020-01-01 00:00:00")]
        [InlineData("2020-01-01T08:00:00", "WA",  "2020-01-01 00:00:00")]

        // To account for DST changes
        [InlineData("2020-07-01T10:00:00", "ACT", "2020-07-01 00:00:00")]
        [InlineData("2020-07-01T10:00:00", "NSW", "2020-07-01 00:00:00")]
        [InlineData("2020-07-01T09:30:00", "NT",  "2020-07-01 00:00:00")]
        [InlineData("2020-07-01T10:00:00", "QLD", "2020-07-01 00:00:00")]
        [InlineData("2020-07-01T09:30:00", "SA",  "2020-07-01 00:00:00")]
        [InlineData("2020-07-01T10:00:00", "TAS", "2020-07-01 00:00:00")]
        [InlineData("2020-07-01T10:00:00", "VIC", "2020-07-01 00:00:00")]
        [InlineData("2020-07-01T08:00:00", "WA",  "2020-07-01 00:00:00")]
        public void LocalisedSendAtTests(string sendAt, string timeZoneId, string expectedResult)
        {
            var result = SMSHelper.LocalisedSendAt(sendAt, timeZoneId, NullLogger.Instance);

            Assert.Equal(expectedResult, result);
        }
    }
}
