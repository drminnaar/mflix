using System;
using HotChocolate.Types;
using MFlix.GqlApi.Movies.Queries.Models;

namespace MFlix.GqlApi.Movies.Queries.Types
{
    public sealed class PageInfoType : ObjectType<PageInfo>
    {
        protected override void Configure(IObjectTypeDescriptor<PageInfo> descriptor)
        {
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            descriptor.Description("Pagination information");
            descriptor.Field(_ => _.CurrentPageNumber).Description("Current page number");
            descriptor.Field(_ => _.HasNext).Description("Are there more pages?");
            descriptor.Field(_ => _.HasPrevious).Description("Are there previous pages?");
            descriptor.Field(_ => _.ItemCount).Description("Total number of items in data source");
            descriptor.Field(_ => _.LastPageNumber).Description("Last page number");
            descriptor.Field(_ => _.NextPageNumber).Description("Next page number");
            descriptor.Field(_ => _.PageCount).Description("Total number of pages");
            descriptor.Field(_ => _.PageSize).Description("Total number of items per page");
            descriptor.Field(_ => _.PreviousPageNumber).Description("Previous page number");
        }
    }
}
