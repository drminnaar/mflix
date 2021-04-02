using System;
using HotChocolate.Types;

namespace MFlix.GqlApi.Movies
{
    public sealed class MovieListType : ObjectType<MovieList>
    {
        protected override void Configure(IObjectTypeDescriptor<MovieList> descriptor)
        {
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            descriptor.Name("movies");
            descriptor.Field(_ => _.Movies).Description("List of movies");
            descriptor.Field(_ => _.PageInfo).Description("Pagination information");
        }
    }
}
