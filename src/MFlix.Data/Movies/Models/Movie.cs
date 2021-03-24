using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MFlix.Data.Movies.Models
{
    public sealed class Movie
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("awards")]
        public AwardsInfo? Awards { get; set; }

        [BsonElement("cast")]
        public IReadOnlyCollection<string>? Cast { get; set; } = Enumerable.Empty<string>().ToList();

        [BsonElement("countries")]
        public IReadOnlyCollection<string> Countries { get; set; } = Enumerable.Empty<string>().ToList();

        [BsonElement("directors")]
        public IReadOnlyCollection<string>? Directors { get; set; } = Enumerable.Empty<string>().ToList();

        [BsonElement("fullplot")]
        public string? FullPlot { get; set; } = string.Empty;

        [BsonElement("genres")]
        public IReadOnlyCollection<string>? Genres { get; set; } = Enumerable.Empty<string>().ToList();

        [BsonElement("imdb")]
        public ImdbRating? Imdb { get; set; }

        [BsonElement("languages")]
        public IReadOnlyCollection<string>? Languages { get; set; } = Enumerable.Empty<string>().ToList();

        [BsonElement("lastupdated")]
        [BsonSerializer(typeof(NullableGeneralDateTimeSerializer))]
        public DateTime? LastUpdated { get; set; }

        [BsonElement("metacritic")]
        public int? Metacritic { get; set; }

        [BsonElement("num_mflix_comments")]
        public int? NumMFlixComments { get; set; }

        [BsonElement("plot")]
        public string? Plot { get; set; } = string.Empty;

        [BsonElement("poster")]
        public string? Poster { get; set; } = string.Empty;

        [BsonElement("rated")]
        public string? Rated { get; set; } = string.Empty;

        [BsonElement("released")]
        [BsonRepresentation(BsonType.String)]
        public DateTime? Released { get; set; }

        [BsonElement("runtime")]
        public int? Runtime { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("tomatoes")]
        public TomatoesRating? Tomatoes { get; set; }

        [BsonElement("type")]
        public string Type { get; set; } = string.Empty;

        [BsonElement("writers")]
        public IReadOnlyCollection<string>? Writers { get; set; } = Enumerable.Empty<string>().ToList();

        [BsonElement("year")]
        [BsonSerializer(typeof(NullableIntSerializer))]
        public int? Year { get; set; }
    }
}
