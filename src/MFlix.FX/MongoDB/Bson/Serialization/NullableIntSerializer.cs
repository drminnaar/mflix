using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Globalization;

namespace MongoDB.Bson.Serialization
{
    public sealed class NullableIntSerializer : NullableSerializer<int>, IRepresentationConfigurable<NullableIntSerializer>
    {
        private readonly BsonType _representation;

        public NullableIntSerializer() : this(BsonType.Int32)
        {
        }

        public NullableIntSerializer(BsonType representation)
        {
            switch (representation)
            {
                case BsonType.String:
                case BsonType.Int32:
                    break;
                default:
                    throw new ArgumentException($"The BsonType '{representation}' is not a valid representation for {GetType().Name}");
            }
            _representation = representation;
        }

        public BsonType Representation => _representation;

        public override int? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var bsonReader = context.Reader;
            var bsonType = bsonReader.GetCurrentBsonType();

            return bsonType switch
            {
                BsonType.String => ParseStringInt(bsonReader.ReadString()),
                BsonType.Int32 => bsonReader.ReadInt32(),
                _ => throw BsonTypeNotSupportedExeption(bsonType)
            };
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, int? value)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var bsonWriter = context.Writer;

            if (value is null)
            {
                bsonWriter.WriteInt32(default);
                return;
            }

            switch (_representation)
            {
                case BsonType.String:
                    bsonWriter.WriteString(value.Value.ToString(CultureInfo.InvariantCulture));
                    break;
                case BsonType.Int32:
                    bsonWriter.WriteInt32(value.Value);
                    break;
                default:
                    throw new BsonSerializationException($"'{_representation}' is not a valid int representation.");
            }
        }

        public NullableIntSerializer WithRepresentation(BsonType representation) =>
            representation == _representation ? this : new NullableIntSerializer(representation);

        IBsonSerializer IRepresentationConfigurable.WithRepresentation(BsonType representation) => WithRepresentation(representation);

        private static int? ParseStringInt(string value)
        {
            if (int.TryParse(value, out var validValue))
                return validValue;

            return default;
        }

        private Exception BsonTypeNotSupportedExeption(BsonType bsonType) => new FormatException(
            $"Deserialization from '{BsonUtils.GetFriendlyTypeName(ValueType)}' to BsonType '{bsonType}' is not supported");
    }
}
