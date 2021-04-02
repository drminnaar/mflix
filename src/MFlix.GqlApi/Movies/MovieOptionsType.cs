using System;
using HotChocolate.Types;

namespace MFlix.GqlApi.Movies
{
    public sealed class MovieOptionsType : ObjectType<MovieOptions>
    {
        protected override void Configure(IObjectTypeDescriptor<MovieOptions> descriptor)
        {
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            descriptor.Name("options");
            descriptor.Description("A set of parameters used to page, sort, and filter movies");
        }
    }
}
