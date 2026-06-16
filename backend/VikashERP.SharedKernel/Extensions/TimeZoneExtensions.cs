using System;

namespace VikashERP.SharedKernel.Extensions;

public static class TimeZoneExtensions
{
    private static readonly TimeZoneInfo IndianTimeZone;

    static TimeZoneExtensions()
    {
        try
        {
            // Windows uses "India Standard Time", Linux/macOS uses "Asia/Kolkata"
            IndianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }
        catch (TimeZoneNotFoundException)
        {
            try
            {
                IndianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            }
            catch (Exception)
            {
                // Fallback to UTC if neither is found
                IndianTimeZone = TimeZoneInfo.Utc;
            }
        }
    }

    public static DateTime ToKolkataTime(this DateTime utcDateTime)
    {
        if (utcDateTime.Kind == DateTimeKind.Unspecified)
        {
            utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        }
        else if (utcDateTime.Kind == DateTimeKind.Local)
        {
            // If it's already local, convert it based on system local time to India time
            return TimeZoneInfo.ConvertTime(utcDateTime, IndianTimeZone);
        }
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, IndianTimeZone);
    }

    public static DateTime? ToKolkataTime(this DateTime? utcDateTime)
    {
        if (!utcDateTime.HasValue) return null;
        return ToKolkataTime(utcDateTime.Value);
    }
}
