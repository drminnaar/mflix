using HotChocolate;

namespace MFlix.GqlApi.Movies.Models
{
    [GraphQLDescription("Tomatoes rating information")]
    public sealed class TomatoesRating
    {
        [GraphQLDescription("Boxoffice information")]
        public string? BoxOffice { get; init; } = string.Empty;

        [GraphQLDescription("Rating consensus")]
        public string? Consensus { get; init; } = string.Empty;

        [GraphQLDescription("Critic rating")]
        public Critic? Critic { get; init; } = new Critic();

        [GraphQLDescription("DVD data")]
        public string? Dvd { get; init; }

        [GraphQLDescription("Fresh information")]
        public int? Fresh { get; init; }

        [GraphQLDescription("Last updated")]
        public string? LastUpdated { get; init; }

        [GraphQLDescription("Producer of movie")]
        public string? Production { get; init; } = string.Empty;

        [GraphQLDescription("Rotten rating")]
        public int? Rotten { get; init; }

        [GraphQLDescription("Viewer rating")]
        public Viewer? Viewer { get; init; } = new Viewer();

        [GraphQLDescription("Website url")]
        public string? Website { get; init; } = string.Empty;
    }
}
