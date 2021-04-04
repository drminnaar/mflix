using HotChocolate;

namespace MFlix.GqlApi.Movies.Mutations.Models
{
    [GraphQLDescription("Represents tomatoes rating information")]
    public sealed class SaveTomatoesInput
    {
        [GraphQLDescription("Unique movie id")]
        public string MovieId { get; set; } = string.Empty;

        [GraphQLDescription("Boxoffice information")]
        public string BoxOffice { get; init; } = string.Empty;

        [GraphQLDescription("Movie consensus")]
        public string Consensus { get; init; } = string.Empty;

        [GraphQLDescription("Rating by critic")]
        public double CriticRating { get; init; }

        [GraphQLDescription("Number of critic reviews")]
        public int CriticNumReviews { get; init; }

        [GraphQLDescription("Critic meter")]
        public int CriticMeter { get; init; }

        [GraphQLDescription("Date of DVD")]
        public string Dvd { get; init; } = string.Empty;

        [GraphQLDescription("Fresh innformation")]
        public int Fresh { get; init; }

        [GraphQLDescription("Last updated")]
        public string LastUpdated { get; init; } = string.Empty;

        [GraphQLDescription("Production information")]
        public string Production { get; init; } = string.Empty;

        [GraphQLDescription("Rotten information")]
        public int Rotten { get; init; }

        [GraphQLDescription("Viewer rating")]
        public double ViewerRating { get; init; }

        [GraphQLDescription("Number of viewer reviews")]
        public int ViewerNumReviews { get; init; }

        [GraphQLDescription("Viewer meter")]
        public int ViewerMeter { get; init; }

        [GraphQLDescription("Link to website")]
        public string Website { get; init; } = string.Empty;
    }
}
