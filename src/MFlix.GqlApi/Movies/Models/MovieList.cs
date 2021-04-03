using System.Collections.Generic;
using System.Linq;

namespace MFlix.GqlApi.Movies.Models
{
    public sealed class MovieList
    {
        public IEnumerable<Movie> Movies { get; init; } = Enumerable.Empty<Movie>();
        public PageInfo PageInfo { get; init; } = PageInfo.Default;
    }
}
