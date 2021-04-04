using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace MFlix.GqlApi.Movies.Mutations
{
    [GraphQLDescription("Represents the data required to create or update a movie")]
    public sealed class SaveMovieInput
    {
        [GraphQLDescription("Unique movie identifier")]
        public string? Id { get; init; } = string.Empty;

        [GraphQLDescription("Name of movie")]
        public string Title { get; init; } = string.Empty;

        [GraphQLDescription("Plot of movie")]
        public string Plot { get; init; } = string.Empty;

        [GraphQLDescription("Length of movie")]
        public int Runtime { get; init; }

        [GraphQLDescription("Parental guidance rating of movie")]
        public string Rated { get; init; } = string.Empty;

        [GraphQLDescription("Year of movie")]
        public int Year { get; init; }

        [GraphQLDescription("Link to movie poster")]
        public string Poster { get; init; } = string.Empty;

        [GraphQLDescription("The date movie was released")]
        public string Released { get; init; } = string.Empty;

        [GraphQLDescription("List of genres")]
        public IReadOnlyCollection<string> Genres { get; init; } = Enumerable.Empty<string>().ToList();

        [GraphQLDescription("List of cast members")]
        public IReadOnlyCollection<string> Cast { get; init; } = Enumerable.Empty<string>().ToList();

        [GraphQLDescription("List of directors")]
        public IReadOnlyCollection<string> Directors { get; init; } = Enumerable.Empty<string>().ToList();
    }
}
