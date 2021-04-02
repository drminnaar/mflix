using System;
using HotChocolate.Types;

namespace MFlix.GqlApi.Movies
{
    public sealed class MovieType : ObjectType<Movie>
    {
        protected override void Configure(IObjectTypeDescriptor<Movie> descriptor)
        {
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            descriptor.Name("movie");
            descriptor.Field(_ => _.Id).Description("Unique movie identifier");
            descriptor.Field(_ => _.Title).Description("Name of movie");
        }
    }
}
