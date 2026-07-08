using System;

namespace VikashERP.Web.Helpers;

public static class DateFormatHelper
{
    public const string DefaultIanaId = "Asia/Kolkata";

    public static string FormatDate(DateTime? utc, string ianaId, string empty = "—")
    {
        if (!utc.HasValue) return empty;
        var tz = GetTimeZone(ianaId);
        var localTime = TimeZoneInfo.ConvertTimeFromUtc(utc.Value, tz);
        return localTime.ToString("dd MMM yyyy");
    }

    public static DateTime ToUtcDate(DateTime localDate, string ianaId)
    {
        var tz = GetTimeZone(ianaId);
        var unspecifiedDate = new DateTime(localDate.Year, localDate.Month, localDate.Day, 0, 0, 0, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(unspecifiedDate, tz);
    }

    public static DateTime GetUserTime(DateTime utc, string ianaId)
    {
        var tz = GetTimeZone(ianaId);
        return TimeZoneInfo.ConvertTimeFromUtc(utc, tz);
    }

    public static string FormatDateTime(DateTime utc, string ianaId)
    {
        var tz = GetTimeZone(ianaId);
        var localTime = TimeZoneInfo.ConvertTimeFromUtc(utc, tz);
        return localTime.ToString("dd MMM yyyy HH:mm");
    }

    private static TimeZoneInfo GetTimeZone(string ianaId)
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(ianaId);
        }
        catch (TimeZoneNotFoundException)
        {
            try
            {
                // Fallback for Windows if IANA ID is used
                if (ianaId == "Asia/Kolkata") return TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                return TimeZoneInfo.Utc;
            }
            catch
            {
                return TimeZoneInfo.Utc;
            }
        }
    }
}
