using Microsoft.Extensions.Logging;
using NodaTime;
using NodaTime.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace WCA.AzureFunctions.EmailToSMS
{
    public static class SMSHelper
    {
        private const string _defaultTimeZoneID = "Australia/Sydney";
        private const string _burstSmsPattern = "uuuu'-'MM'-'dd' 'HH':'mm':'ss";


        public static string CleanMobileNumber(string mobileNumber)
        {
            if (string.IsNullOrEmpty(mobileNumber))
            {
                return mobileNumber;
            }

            var invalidMobileCharsPattern = new Regex(@"\(*\)*-*\s*", RegexOptions.Compiled);

            return invalidMobileCharsPattern.Replace(mobileNumber, "");
        }

        public static string LocalisedSendAt(string sendAt, string sendAtTimeZoneID, ILogger logger)
        {
            if (string.IsNullOrEmpty(sendAt))
            {
                return sendAt;
            }

            var localDateTimeParseResult = LocalDateTimePattern.GeneralIso.Parse(sendAt);

            // If parsing failed, try without seconds in case they were left out
            if (!localDateTimeParseResult.Success)
            {
                var LocalDateTimePatternGeneralIsoWithoutSeconds =
                    LocalDateTimePattern.CreateWithInvariantCulture("uuuu'-'MM'-'dd'T'HH':'mm");
                localDateTimeParseResult = LocalDateTimePatternGeneralIsoWithoutSeconds.Parse(sendAt);
            }

            // If parsing still didn't work, check to see if it has an offset (Zulu or otherwise)
            if (!localDateTimeParseResult.Success)
            {
                var offsetDateTimeParseResult = OffsetDateTimePattern.GeneralIso.Parse(sendAt);
                if (offsetDateTimeParseResult.Success)
                {
                    return offsetDateTimeParseResult.Value.WithOffset(Offset.Zero).ToString(_burstSmsPattern, CultureInfo.InvariantCulture);
                }
            }

            if (!localDateTimeParseResult.Success)
            {
                // Default back to original string if it can't be parsed 
                return sendAt;
            }

            var resolvedDateTimeZone = GetZoneOrDefault(sendAtTimeZoneID, logger);

            var zonedDateTime = localDateTimeParseResult.Value.InZoneLeniently(resolvedDateTimeZone);

            return zonedDateTime.WithZone(DateTimeZone.Utc).ToString(_burstSmsPattern, CultureInfo.InvariantCulture);
        }

        private static DateTimeZone GetZoneOrDefault(string timeZoneID, ILogger logger)
        {
            DateTimeZone resolvedDateTimeZone = null;

            if (!string.IsNullOrEmpty(timeZoneID))
            {
                // First try to resolve time zone by ID
                resolvedDateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneID);

                // If that failed, then try to look up based on Australian State
                if (resolvedDateTimeZone is null)
                {
                    switch (timeZoneID.Trim().ToUpper(CultureInfo.InvariantCulture))
                    {
                        case "ACT":
                            resolvedDateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull("Australia/Sydney");
                            break;
                        case "NSW":
                            resolvedDateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull("Australia/Sydney");
                            break;
                        case "NT":
                            resolvedDateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull("Australia/Darwin");
                            break;
                        case "QLD":
                            resolvedDateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull("Australia/Brisbane");
                            break;
                        case "SA":
                            resolvedDateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull("Australia/Adelaide");
                            break;
                        case "TAS":
                            resolvedDateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull("Australia/Hobart");
                            break;
                        case "VIC":
                            resolvedDateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull("Australia/Melbourne");
                            break;
                        case "WA":
                            resolvedDateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull("Australia/Perth");
                            break;
                        default:
                            break;
                    }
                }
            }

            if (resolvedDateTimeZone is null)
            {
                resolvedDateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(_defaultTimeZoneID);
                logger.LogWarning(
                    "Reverting to default time zone ID '{DefaultTimeZoneID}' because the supplied time zone ID was missing or invalid: 'SuppliedTimeZoneID'.",
                    _defaultTimeZoneID,
                    timeZoneID);
            }

            return resolvedDateTimeZone;
        }
    }
}
