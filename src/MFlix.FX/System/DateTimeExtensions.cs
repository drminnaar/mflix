using System.Globalization;

namespace System
{
    public static class DateTimeExtensions
    {
        public static string ToInvariantString(this DateTime date, string? format = null)
        {
            return date.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
