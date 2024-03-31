using TimeZoneConverter;

namespace WebAPI.Extensions;

public static class TimeZoneInfoExtensions
{
    public static string GetLocalTZIdentifier(this TimeZoneInfo timeZoneInfo)
    {
        return TZConvert.WindowsToIana(timeZoneInfo.Id);
    }
}
