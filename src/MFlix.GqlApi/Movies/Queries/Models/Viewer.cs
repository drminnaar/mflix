using HotChocolate;

namespace MFlix.GqlApi.Movies.Queries.Models
{
    [GraphQLDescription("Viewer rating information")]
    public sealed class Viewer
    {
        [GraphQLDescription("Viewer rating")]
        public double? Rating { get; init; }

        [GraphQLDescription("Number of reviews")]
        public int? NumReviews { get; init; }

        [GraphQLDescription("Review meter")]
        public int? Meter { get; init; }
    }
}
