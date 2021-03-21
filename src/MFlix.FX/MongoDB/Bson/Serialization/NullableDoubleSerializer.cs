using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Globalization;

namespace MongoDB.Bson.Serialization
{
    public sealed class NullableDoubleSerializer : NullableSerializer<double>, IRepresentationConfigurable<NullableDoubleSerializer>
    {
        private readonly BsonType _representation;

        public NullableDoubleSerializer() : this(BsonType.Double)
        {
        }

        public NullableDoubleSerializer(BsonType representation)
        {
            switch (representation)
            {
                case BsonType.String:
                case BsonType.Double:
                    break;
                default:
                    throw new ArgumentException($"The BsonType '{representation}' is not a valid representation for {GetType().Name}");
            }
            _representation = representation;
        }

        public BsonType Representation => _representation;

        public override double? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var bsonReader = context.Reader;
            var bsonType = bsonReader.GetCurrentBsonType();

            return bsonType switch
            {
                BsonType.String => ParseStringDouble(bsonReader.ReadString()),
                BsonType.Double => bsonReader.ReadDouble(),
                _ => throw BsonTypeNotSupportedExeption(bsonType)
            };
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, double? value)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var bsonWriter = context.Writer;

            if (value is null)
            {
                bsonWriter.WriteDouble(default);
                return;
            }

            switch (_representation)
            {
                case BsonType.String:
                    bsonWriter.WriteString(value.Value.ToString(CultureInfo.InvariantCulture));
                    break;
                case BsonType.Double:
                    bsonWriter.WriteDouble(value.Value);
                    break;
                default:
                    throw new BsonSerializationException($"'{_representation}' is not a valid double representation.");
            }
        }

        public NullableDoubleSerializer WithRepresentation(BsonType representation) =>
            representation == _representation ? this : new NullableDoubleSerializer(representation);

        IBsonSerializer IRepresentationConfigurable.WithRepresentation(BsonType representation) => WithRepresentation(representation);

        private static double? ParseStringDouble(string value)
        {
            if (double.TryParse(value, out var validValue))
                return validValue;

            return default;
        }

        private Exception BsonTypeNotSupportedExeption(BsonType bsonType) => new FormatException(
            $"Deserialization from '{BsonUtils.GetFriendlyTypeName(ValueType)}' to BsonType '{bsonType}' is not supported");
    }
}
