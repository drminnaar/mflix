using System;
using HotChocolate.Types;
using MFlix.GqlApi.Movies.Queries.Models;

namespace MFlix.GqlApi.Movies.Queries.Types
{
    public sealed class MovieListType : ObjectType<MovieList>
    {
        protected override void Configure(IObjectTypeDescriptor<MovieList> descriptor)
        {
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            descriptor.Field(_ => _.Movies).Description("List of movies");
            descriptor.Field(_ => _.PageInfo).Description("Pagination information");
        }
    }
}
