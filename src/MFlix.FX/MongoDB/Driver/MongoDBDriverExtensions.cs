using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDB.Driver
{
    public static class MongoDBDriverExtensions
    {
        public static Task<IPagedCollection<TProjection>> ToPagedCollectionAsync<TDocument, TProjection>(
            this IFindFluent<TDocument, TProjection> source,
            int pageNumber,
            int pageSize)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            async Task<IPagedCollection<TProjection>> ToPagedCollectionAsync()
            {
                var itemCount = await source.CountDocumentsAsync().ConfigureAwait(false);

                var items = await source
                    .Skip(pageSize * (pageNumber - 1))
                    .Limit(pageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return new PagedCollection<TProjection>(items, itemCount, pageNumber, pageSize);
            }

            return ToPagedCollectionAsync();
        }
    }
}
