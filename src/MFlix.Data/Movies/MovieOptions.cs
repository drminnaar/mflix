using System.Collections.Generic;
using System.Linq;

namespace MFlix.Data.Movies
{
    public sealed record MovieOptions
    {
        public static MovieOptions Empty(int pageNumber, int pageSize) =>
            new() { PageNumber = pageNumber, PageSize = pageSize };

        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public string Rated { get; init; } = string.Empty;
        public string Runtime { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Year { get; init; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public IReadOnlyCollection<string> SortBy { get; init; } = Enumerable.Empty<string>().ToList();
        public IReadOnlyCollection<string> Cast { get; init; } = Enumerable.Empty<string>().ToList();
        public IReadOnlyCollection<string> Genres { get; init; } = Enumerable.Empty<string>().ToList();
        public IReadOnlyCollection<string> Directors { get; init; } = Enumerable.Empty<string>().ToList();
    }
}
