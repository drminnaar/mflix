using HotChocolate;

namespace MFlix.GqlApi.Movies.Models
{
    [GraphQLDescription("Represents a set of options to filter, sort, and page movie data")]
    public sealed class MovieOptions
    {
        private const int DefaultPageNumber = 1;
        private const int DefaultPageSize = 10;

        public static MovieOptions? Default => new();

        [GraphQLDescription("Page number")]
        public int? Page { get; init; } = DefaultPageNumber;

        [GraphQLDescription("Limit number of movies returned by specifying a page size (the number of movies per page). eg. 100 = 100 movies per pag")]
        public int? Limit { get; init; } = DefaultPageSize;

        [GraphQLDescription("Filter movies by parental guidance rating. eg PG, PG-10")]
        public string? Rated { get; init; } = string.Empty;

        [GraphQLDescription("Filter movies by length of movie. eg. gt:120, gte:120, lt:120, lte:120, eq:120")]
        public string? Runtime { get; init; } = string.Empty;

        [GraphQLDescription("Filter movies by movie name. Partial names are accepted.")]
        public string? Title { get; init; } = string.Empty;

        [GraphQLDescription("Filter movies by year of movie. eg. gt:2015, gte:2015, lt:2015, lte:2015, eq:2015")]
        public string? Year { get; init; } = string.Empty;

        [GraphQLDescription("Filter movies by type. eg. Movie or Series")]
        public string? Type { get; init; } = string.Empty;

        [GraphQLDescription("Sort movies by specified comma separated order. eg. title (order by title ascending) OR title,-year (order by title ascending, year descending)")]
        public string? Order { get; init; } = string.Empty;

        [GraphQLDescription("Filter movies by specifying comma separated list of cast members. eg. tom cruise, harrison ford")]
        public string? Cast { get; init; } = string.Empty;

        [GraphQLDescription("Filter movies by specifying comma separated list of genres. eg. action,adventure")]
        public string? Genres { get; init; } = string.Empty;

        [GraphQLDescription("Filter movies by specifying comma separated list of directors. eg. david cameron, steven spielberg")]
        public string? Directors { get; init; } = string.Empty;
    }
}
