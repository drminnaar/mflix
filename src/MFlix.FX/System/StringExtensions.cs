using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class StringExtensions
    {
        public static IReadOnlyCollection<string> FromCsv(this string csv)
        {
            return csv
                ?.Split
                (
                    new[] { "," },
                    StringSplitOptions.RemoveEmptyEntries
                )
                ?.Select(_ => _.Trim())
                ?.ToArray()
                ?? Array.Empty<string>();
        }
    }
}
