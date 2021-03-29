using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MFlix.HttpApi.Models
{
    [DataContract(Namespace = "")]
    public sealed class Movie
    {
        [DataMember]
        public string Id { get; init; } = string.Empty;

        [DataMember]
        public IEnumerable<Cast>? CastMembers { get; set; } = Enumerable.Empty<Cast>().ToList();

        [DataMember]
        public IEnumerable<Director>? Directors { get; init; } = Enumerable.Empty<Director>().ToList();

        [DataMember]
        public IEnumerable<Genre>? Genres { get; init; } = Enumerable.Empty<Genre>().ToList();

        [DataMember]
        public ImdbRating? Imdb { get; init; }

        [DataMember]
        public string? Poster { get; init; } = string.Empty;

        [DataMember]
        public string? Rated { get; init; } = string.Empty;

        [DataMember]
        public int? Runtime { get; init; }

        [DataMember]
        public string Title { get; init; } = string.Empty;

        [DataMember]
        public TomatoesRating? Tomatoes { get; init; }

        [DataMember]
        public int? Year { get; init; }
    }
}
