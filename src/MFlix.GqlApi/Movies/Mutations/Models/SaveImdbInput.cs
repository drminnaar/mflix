using HotChocolate;

namespace MFlix.GqlApi.Movies.Mutations.Models
{
    [GraphQLDescription("Represents data to create or update IMDB information")]
    public sealed class SaveImdbInput
    {
        [GraphQLDescription("Unique movie id")]
        public string MovieId { get; init; } = string.Empty;

        [GraphQLDescription("Unique IMDB id")]
        public int Id { get; init; }

        [GraphQLDescription("IMDB rating")]
        public double? Rating { get; init; }

        [GraphQLDescription("IMDB votes")]
        public int? Votes { get; init; }
    }
}
