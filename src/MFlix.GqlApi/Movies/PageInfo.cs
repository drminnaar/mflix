namespace MFlix.GqlApi.Movies
{
    public sealed class PageInfo
    {
        public static PageInfo Default => new();

        public int CurrentPageNumber { get; init; }
        public int? NextPageNumber { get; init; }
        public int? PreviousPageNumber { get; init; }
        public int LastPageNumber { get; init; }
        public long ItemCount { get; init; }
        public int PageSize { get; init; }
        public int PageCount { get; init; }
        public bool HasPrevious { get; init; }
        public bool HasNext { get; init; }
    }
}
