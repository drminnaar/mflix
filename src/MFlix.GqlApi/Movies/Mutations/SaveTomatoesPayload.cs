using HotChocolate;

namespace MFlix.GqlApi.Movies.Mutations
{
    [GraphQLDescription("Represents data resulting from saving tomatoes rating information")]
    public sealed class SaveTomatoesPayload
    {
        [GraphQLDescription("Rating by critic")]
        public double CriticRating { get; init; }

        [GraphQLDescription("Number of critic reviews")]
        public int CriticNumReviews { get; init; }

        [GraphQLDescription("Critic meter")]
        public int CriticMeter { get; init; }

        [GraphQLDescription("Viewer rating")]
        public double ViewerRating { get; init; }

        [GraphQLDescription("Number of viewer reviews")]
        public int ViewerNumReviews { get; init; }

        [GraphQLDescription("Viewer meter")]
        public int ViewerMeter { get; init; }
    }
}
