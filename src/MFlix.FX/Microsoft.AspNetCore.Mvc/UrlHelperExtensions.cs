using System;
using System.Reflection;
using System.Web;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static Uri? LinkCurrentPage<T>(
            this IUrlHelper urlHelper,
            string routeName,
            int pageSize,
            int currentPageNumber,
            T queryParams) where T : PagedQueryParams
        {
            if (currentPageNumber < 1 || pageSize < 1)
                return null;

            return urlHelper.Link<T>(routeName, currentPageNumber, queryParams);
        }

        public static Uri? LinkFirstPage<T>(
            this IUrlHelper urlHelper,
            string routeName,
            int pageSize,
            T queryParams) where T : PagedQueryParams
        {
            if (pageSize < 1)
                return null;

            return urlHelper.Link<T>(routeName, 1, queryParams);
        }

        public static Uri? LinkLastPage<T>(
            this IUrlHelper urlHelper,
            string routeName,
            int pageSize,
            int lastPageNumber,
            T queryParams) where T : PagedQueryParams
        {
            if (lastPageNumber < 1 || pageSize < 1)
                return null;

            return urlHelper.Link<T>(routeName, lastPageNumber, queryParams);
        }

        public static Uri? LinkNextPage<T>(
            this IUrlHelper urlHelper,
            string routeName,
            int pageSize,
            int? nextPageNumber,
            T queryParams) where T : PagedQueryParams
        {
            if (!nextPageNumber.HasValue || nextPageNumber < 1 || pageSize < 1)
                return null;

            return urlHelper.Link<T>(routeName, nextPageNumber.Value, queryParams);
        }

        public static Uri? LinkPreviousPage<T>(
            this IUrlHelper urlHelper,
            string routeName,
            int pageSize,
            int? previousPageNumber,
            T queryParams) where T : PagedQueryParams
        {
            if (!previousPageNumber.HasValue || previousPageNumber < 1 || pageSize < 1)
                return null;

            return urlHelper.Link<T>(routeName, previousPageNumber.Value, queryParams);
        }

        public static Uri? Link<T>(
            this IUrlHelper urlHelper,
            string routeName,
            int pageNumber,
            T queryParams) where T : PagedQueryParams
        {
            if (urlHelper is null)
                throw new ArgumentNullException(nameof(urlHelper));

            if (string.IsNullOrEmpty(routeName))
                throw new ArgumentException("Expected non-null/empty route name", nameof(routeName));

            if (queryParams is null)
                throw new ArgumentNullException(nameof(queryParams));

            var routeValues = queryParams.ToRouteValuesDictionary();
            routeValues[PageNumberParam<T>().Name] = pageNumber;
            var uriString = HttpUtility.UrlDecode(urlHelper.Link(routeName, routeValues));
            return string.IsNullOrWhiteSpace(uriString) ? null : new Uri(uriString);
        }

        private static FromQueryAttribute PageNumberParam<T>()
        {
            return typeof(T)
                .GetProperty(nameof(PagedQueryParams.PageNumber), BindingFlags.Public | BindingFlags.Instance)
                ?.GetCustomAttribute(typeof(FromQueryAttribute))
                as FromQueryAttribute
                ?? throw new InvalidOperationException($"Property '{nameof(PagedQueryParams.PageNumber)}'"
                    + $" must have attribute of type '{nameof(FromQueryAttribute)}'.");
        }
    }
}
