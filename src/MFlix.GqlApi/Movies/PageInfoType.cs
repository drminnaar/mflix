using System;
using HotChocolate.Types;

namespace MFlix.GqlApi.Movies
{
    public sealed class PageInfoType : ObjectType<PageInfo>
    {
        protected override void Configure(IObjectTypeDescriptor<PageInfo> descriptor)
        {
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            descriptor.Description("Pagination information");
        }
    }
}
