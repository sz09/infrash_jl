using System;
using TimeZoneConverter;

namespace JobLogic.Infrastructure.CultureProvider
{
    public static class TimeZoneHelper
    {
        public static string MapWindowsTimeZoneToIANATimeZone(this string windowsTimezone)
        {
            return (!string.IsNullOrWhiteSpace(windowsTimezone)) ? TZConvert.WindowsToIana(windowsTimezone) : string.Empty;
        }

        public static string MapIANATimeZoneToWindowsTimeZone(this string IANATimeZone)
        {
            return (!string.IsNullOrWhiteSpace(IANATimeZone)) ? TZConvert.IanaToWindows(IANATimeZone) : string.Empty;
        }

        public static bool TryGetTimeZoneInfo(this string windowsOrIanaTimeZoneId, out TimeZoneInfo timeZoneInfo)
        {
            if (!string.IsNullOrWhiteSpace(windowsOrIanaTimeZoneId))
               return TZConvert.TryGetTimeZoneInfo(windowsOrIanaTimeZoneId, out timeZoneInfo);

            timeZoneInfo = null;
            return false;
        }
    }
}