using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MFlix.DataTests.Infrastructure
{
    public class DateTimeConverterUsingDateTimeParse : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString() ?? string.Empty, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer?.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
