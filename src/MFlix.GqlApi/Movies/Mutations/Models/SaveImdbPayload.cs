using HotChocolate;

namespace MFlix.GqlApi.Movies.Mutations.Models
{
    [GraphQLDescription("Represents data resulting from saving IMDB information")]
    public sealed class SaveImdbPayload
    {
        [GraphQLDescription("Unique IMDB id")]
        public int Id { get; init; }

        [GraphQLDescription("IMDB rating")]
        public double? Rating { get; init; }

        [GraphQLDescription("IMDB votes")]
        public int? Votes { get; init; }
    }
}
