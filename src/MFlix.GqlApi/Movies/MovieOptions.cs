namespace MFlix.GqlApi.Movies
{
    public sealed class MovieOptions
    {
        public int? PageNumber { get; init; }
        public int? PageSize { get; init; }
        public string? Rated { get; init; } = string.Empty;
        public string? Runtime { get; init; } = string.Empty;
        public string? Title { get; init; } = string.Empty;
        public string? Year { get; init; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public string? OrderBy { get; init; } = string.Empty;
        public string? Cast { get; init; } = string.Empty;
        public string? Genres { get; init; } = string.Empty;
        public string? Directors { get; init; } = string.Empty;
    }
}
