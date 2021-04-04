using HotChocolate;

namespace MFlix.GqlApi.Movies.Queries.Models
{
    [GraphQLDescription("Critic rating information")]
    public sealed class Critic
    {
        [GraphQLDescription("Critic rating")]
        public double? Rating { get; init; }

        [GraphQLDescription("Number of reviews")]
        public int? NumReviews { get; init; }

        [GraphQLDescription("Critic meter")]
        public int? Meter { get; init; }
    }
}
