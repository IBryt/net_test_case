using GeoTimeZone;
using TimeZoneConverter;


namespace WebAPI.Extensions;

public static class StringExtensions
{
    public static DateTime GetTime(this string coordinates, DateTime utcDateTime)
    {
        var latlng = coordinates.Split(',')
                .Select(x => Convert.ToDouble(x.Trim().Replace(".", ",")))
                .ToArray();

        string tzIana = TimeZoneLookup.GetTimeZone(latlng[0], latlng[1]).Result;
        TimeZoneInfo tzInfo = TZConvert.GetTimeZoneInfo(tzIana);
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, tzInfo);
    }

    public static string GetTimeZone(this string coordinates, DateTime utcDateTime)
    {
        var latlng = coordinates.Split(',')
                .Select(x => Convert.ToDouble(x.Trim().Replace(".", ",")))
                .ToArray();

        return TimeZoneLookup.GetTimeZone(latlng[0], latlng[1]).Result;
    }
}
