using GeoTimeZone;

namespace WebAPI.Extensions;

public static class StringExtensions
{
    public static string GetTimeZone(this string coordinates, DateTime utcDateTime)
    {
        var latlng = coordinates.Split(',')
                .Select(x => Convert.ToDouble(x.Trim().Replace(".", ",")))
                .ToArray();

        return TimeZoneLookup.GetTimeZone(latlng[0], latlng[1]).Result;
    }
}
