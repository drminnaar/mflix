using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Primitives;

namespace Microsoft.AspNetCore.Mvc
{
    public sealed class PaginationHeader
    {
        private const string HEADER_NAME = "X-Pagination";

        public PaginationHeader(
            int currentPageNumber,
            int pageSize,
            int pageCount,
            long itemCount,
            Uri? nextPageUrl,
            Uri? previousPageUrl,
            Uri? firstPageUrl,
            Uri? lastPageUrl,
            Uri? currentPageUrl)
        {
            CurrentPageNumber = currentPageNumber;
            PageSize = pageSize;
            PageCount = pageCount;
            ItemCount = itemCount;
            NextPageUrl = nextPageUrl;
            PreviousPageUrl = previousPageUrl;
            FirstPageUrl = firstPageUrl;
            LastPageUrl = lastPageUrl;
            CurrentPageUrl = currentPageUrl;
        }

        public int CurrentPageNumber { get; }
        public long ItemCount { get; }
        public int PageSize { get; }
        public int PageCount { get; }
        public Uri? FirstPageUrl { get; }
        public Uri? LastPageUrl { get; }
        public Uri? NextPageUrl { get; }
        public Uri? PreviousPageUrl { get; }
        public Uri? CurrentPageUrl { get; }

        public string ToJsonString()
        {
            // I'm accepting the risk of unsafe encoder
            // see official documentation https://bit.ly/2PT7VmP
            return JsonSerializer.Serialize(
                value: this,
                options: new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        }

        public KeyValuePair<string, StringValues> ToKeyValuePair() =>
            new(HEADER_NAME, ToJsonString());
    }
}
