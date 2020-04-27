using System;

namespace WCA.Core.Features.Actionstep
{
    internal static class ActionstepUtilities
    {

        internal static string GetActionstepFormattedStringValue(object value)
        {
            if (value == null)
            {
                return null;
            }

            switch (value)
            {
                case bool boolValue:
                    return boolValue ? "on" : "off";
                case DateTime dateTimeValue:
                    return dateTimeValue.ToString("yyyy-MM-dd HH:mm");
                default:
                    return value.ToString();
            }
        }
    }
}
