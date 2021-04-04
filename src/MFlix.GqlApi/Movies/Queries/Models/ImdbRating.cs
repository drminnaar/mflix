using HotChocolate;

namespace MFlix.GqlApi.Movies.Queries.Models
{
    [GraphQLDescription("IMDB rating information")]
    public sealed class ImdbRating
    {
        [GraphQLDescription("IMDB unique identifier")]
        public int Id { get; init; }

        [GraphQLDescription("IMDB rating")]
        public double? Rating { get; init; }

        [GraphQLDescription("Number of votes")]
        public int? Votes { get; init; }
    }
}
