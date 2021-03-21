using MongoDB.Bson.Serialization.Attributes;

namespace MFlix.Data.Movies.Models
{
    public sealed class ViewerInfo
    {
        [BsonElement("rating")]
        public double? Rating { get; set; }

        [BsonElement("numReviews")]
        public int? NumReviews { get; set; }

        [BsonElement("meter")]
        public int? Meter { get; set; }
    }
}
