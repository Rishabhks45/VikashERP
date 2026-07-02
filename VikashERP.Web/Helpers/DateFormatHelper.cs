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
