using System;
using System.Globalization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoDB.Bson.Serialization
{
    public sealed class NullableGeneralDateTimeSerializer : NullableSerializer<DateTime>, IRepresentationConfigurable<NullableGeneralDateTimeSerializer>
    {
        private const string StringSerializationFormat = "g";
        private readonly BsonType _representation;

        public NullableGeneralDateTimeSerializer() : this(BsonType.DateTime)
        {
        }

        public NullableGeneralDateTimeSerializer(BsonType representation)
        {
            switch (representation)
            {
                case BsonType.String:
                case BsonType.DateTime:
                    break;
                default:
                    throw new ArgumentException($"The BsonType '{representation}' is not a valid representation for {GetType().Name}");
            }
            _representation = representation;
        }

        public BsonType Representation => _representation;

        public override DateTime? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var bsonReader = context.Reader;
            var bsonType = bsonReader.GetCurrentBsonType();

            return bsonType switch
            {
                BsonType.String => ParseStringDate(bsonReader.ReadString()),
                BsonType.DateTime => DateTimeOffset.FromUnixTimeMilliseconds(bsonReader.ReadDateTime()).DateTime,
                _ => throw BsonTypeNotSupportedExeption(bsonType)
            };
        }

        private static DateTime? ParseStringDate(string date)
        {
            if (DateTime.TryParse(date, out var validDate))
                return validDate;

            return default;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime? value)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var bsonWriter = context.Writer;

            if (value is null)
            {
                bsonWriter.WriteDateTime(default);
                return;
            }

            switch (_representation)
            {
                case BsonType.String:
                    bsonWriter.WriteString(value.Value.ToString(StringSerializationFormat, DateTimeFormatInfo.InvariantInfo));
                    break;
                case BsonType.DateTime:
                    bsonWriter.WriteDateTime(new DateTimeOffset(value.Value).ToUnixTimeMilliseconds());
                    break;
                default:
                    throw new BsonSerializationException($"'{_representation}' is not a valid DateTime representation.");
            }
        }

        public NullableGeneralDateTimeSerializer WithRepresentation(BsonType representation) =>
            representation == _representation ? this : new NullableGeneralDateTimeSerializer(representation);

        IBsonSerializer IRepresentationConfigurable.WithRepresentation(BsonType representation) => WithRepresentation(representation);

        private Exception BsonTypeNotSupportedExeption(BsonType bsonType) => new FormatException(
            $"Deserialization from '{BsonUtils.GetFriendlyTypeName(ValueType)}' to BsonType '{bsonType}' is not supported");
    }
}
