using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace MFlix.GqlApi.Movies.Models
{
    [GraphQLDescription("Movie information")]
    public sealed class Movie
    {
        [GraphQLDescription("Unique movie identifier")]
        public string Id { get; init; } = string.Empty;

        [GraphQLDescription("List of cast members")]
        public IEnumerable<string>? Cast { get; set; } = Enumerable.Empty<string>().ToList();

        [GraphQLDescription("List of directors")]
        public IEnumerable<string>? Directors { get; init; } = Enumerable.Empty<string>().ToList();

        [GraphQLDescription("List of genres")]
        public IEnumerable<string>? Genres { get; init; } = Enumerable.Empty<string>().ToList();

        [GraphQLDescription("IMDB rating")]
        public ImdbRating? Imdb { get; init; }

        [GraphQLDescription("Poster url")]
        public string? Poster { get; init; } = string.Empty;

        [GraphQLDescription("Rated (parental guidance rating")]
        public string? Rated { get; init; } = string.Empty;

        [GraphQLDescription("Length of movie")]
        public int? Runtime { get; init; }

        [GraphQLDescription("Name of movie")]
        public string Title { get; init; } = string.Empty;

        [GraphQLDescription("Tomatoes rating")]
        public TomatoesRating? Tomatoes { get; init; }

        [GraphQLDescription("Year of movie")]
        public int? Year { get; init; }
    }
}
