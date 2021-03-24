using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MFlix.Data.Movies.Models
{
    public sealed class TomatoesRating
    {
        [BsonElement("boxOffice")]
        public string? BoxOffice { get; set; } = string.Empty;

        [BsonElement("consensus")]
        public string? Consensus { get; set; } = string.Empty;

        [BsonElement("critic")]
        public CriticInfo? Critic { get; set; } = new CriticInfo();

        [BsonElement("dvd")]
        [BsonDateTimeOptions(DateOnly = true, Kind = DateTimeKind.Utc)]
        public DateTime? Dvd { get; set; }

        [BsonElement("fresh")]
        public int? Fresh { get; set; }

        [BsonElement("lastUpdated")]
        public DateTime LastUpdated { get; set; }

        [BsonElement("production")]
        public string? Production { get; set; } = string.Empty;

        [BsonElement("rotten")]
        public int? Rotten { get; set; }

        [BsonElement("viewer")]
        public ViewerInfo? Viewer { get; set; } = new ViewerInfo();

        [BsonElement("website")]
        public string? Website { get; set; } = string.Empty;
    }
}
