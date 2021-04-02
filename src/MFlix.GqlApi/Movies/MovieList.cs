using System.Collections.Generic;
using System.Linq;

namespace MFlix.GqlApi.Movies
{
    public sealed class MovieList
    {
        public IEnumerable<Movie> Movies { get; init; } = Enumerable.Empty<Movie>();
        public PageInfo PageInfo { get; init; } = PageInfo.Default;
    }
}
