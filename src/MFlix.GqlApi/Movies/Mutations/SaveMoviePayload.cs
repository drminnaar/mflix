using HotChocolate;

namespace MFlix.GqlApi.Movies.Mutations
{
    [GraphQLDescription("Represents the data resulting from saving a movie")]
    public sealed class SaveMoviePayload
    {
        [GraphQLDescription("Unique movie id")]
        public string MovieId { get; init; } = string.Empty;
    }
}
