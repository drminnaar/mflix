using System.Globalization;

namespace System
{
    public static class DateTimeOffsetExtensions
    {
        public static string ToInvariantString(this DateTimeOffset date, string? format = null)
        {
            return date.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
