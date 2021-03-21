using MongoDB.Bson.Serialization.Attributes;

namespace MFlix.Data.Movies.Models
{
    public sealed class AwardsInfo
    {
        [BsonElement("nominations")]
        public int Nominations { get; set; }

        [BsonElement("text")]
        public string Text { get; set; } = string.Empty;

        [BsonElement("wins")]
        public int Wins { get; set; }
    }
}
