using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerExtensions
    {
        public static IActionResult OkWithPageHeader<T>(
            this ControllerBase controller,
            IPagedCollection<T> items,
            string routeName,
            PagedQueryParams queryParams,
            IUrlHelper urlHelper)
        {
            if (controller is null)
                throw new ArgumentNullException(nameof(controller));

            if (items is null)
                throw new ArgumentNullException(nameof(items));

            if (string.IsNullOrEmpty(routeName))
                throw new ArgumentException("Expected non-null/empty route name", nameof(routeName));

            if (queryParams is null)
                throw new ArgumentNullException(nameof(queryParams));

            if (urlHelper is null)
                throw new ArgumentNullException(nameof(urlHelper));

            controller
                .Response
                .Headers
                .Add(items.ToPaginationHeader(routeName, queryParams, urlHelper).ToKeyValuePair());

            return controller.Ok(items);
        }
    }
}
