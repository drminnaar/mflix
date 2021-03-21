using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace MFlix.Data.Movies.Models
{
    [BsonNoId]
    public sealed class ImdbRating
    {
        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("rating")]
        [BsonSerializer(typeof(NullableDoubleSerializer))]
        public double? Rating { get; set; }

        [BsonElement("votes")]
        [BsonSerializer(typeof(NullableIntSerializer))]
        public int? Votes { get; set; }
    }
}
