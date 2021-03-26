using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MFlix.Data.Movies.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MFlix.Data.Movies
{
    public interface IMovieDao
    {
        Task<Movie> DeleteMovie(string movieId);
        Task<Movie> GetMovieById(ObjectId movieId);
        Task<Movie> GetMovieById(string movieId);
        Task<IPagedCollection<Movie>> GetMovies(MovieOptions options);
        Task<ImdbRating> SaveImdbRating(string movieId, ImdbRating rating);
        Task<TomatoesRating> SaveTomatoesRating(string movieId, TomatoesRating rating);
        Task<ObjectId> SaveMovie(Movie movie);
        Task<int?> SaveMetacriticRating(string movieId, int? rating);
    }

    public sealed class MovieDao : IMovieDao
    {
        private readonly IMongoCollection<Movie> _collection;
        private readonly IMovieFilterBuilder _filter;
        private readonly IMovieSortDefinition _sorter;

        public MovieDao(
            IMongoCollection<Movie> collection,
            IMovieFilterBuilder filter,
            IMovieSortDefinition sorter)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _filter = filter ?? throw new ArgumentNullException(nameof(filter));
            _sorter = sorter ?? throw new ArgumentNullException(nameof(sorter));
        }

        public Task<Movie> DeleteMovie(string movieId)
        {
            if (!ObjectId.TryParse(movieId, out var movieIdValue))
                throw NewInvalidMovieIdArgumentException(movieId);

            return DeleteMovie();

            async Task<Movie> DeleteMovie()
            {
                var filter = Builders<Movie>.Filter.Eq(movie => movie.Id, new ObjectId(movieId));

                return await _collection
                    .FindOneAndDeleteAsync(filter)
                    .ConfigureAwait(false)
                    ?? throw NewEntityNotFoundException(nameof(DeleteMovie), movieId);
            };
        }

        public Task<Movie> GetMovieById(string movieId)
        {
            if (!ObjectId.TryParse(movieId, out var movieIdValue))
                throw NewInvalidMovieIdArgumentException(movieId);

            return GetMovieById(movieIdValue);
        }

        public Task<Movie> GetMovieById(ObjectId movieId) => _collection
                .Find(movie => movie.Id == movieId)
                .FirstOrDefaultAsync()
                ?? throw NewEntityNotFoundException(nameof(GetMovieById), movieId.ToString());

        public Task<IPagedCollection<Movie>> GetMovies(MovieOptions options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            var filter = _filter
                .ClearFilters()
                .WhereCastInAny(options.Cast)
                .WhereDirectorsInAny(options.Directors)
                .WhereGenresInAny(options.Genres)
                .WhereRatedEquals(options.Rated)
                .WhereRuntime(options.Runtime)
                .WhereTitleLike(options.Title)
                .WhereTypeEquals(options.Type)
                .WhereYear(options.Year)
                .BuildAsLogicalAnd();

            var sortDefinition = _sorter.SortBy(options.SortBy);

            return _collection
                .Find(filter)
                .Sort(sortDefinition)
                .ToPagedCollectionAsync(options.PageNumber, options.PageSize);
        }

        public Task<ObjectId> SaveMovie(Movie movie)
        {
            if (movie is null)
                throw new ArgumentNullException(nameof(movie));

            return SaveMovie();

            async Task<ObjectId> SaveMovie()
            {
                await _collection
                    .ReplaceOneAsync
                    (
                        filter: Builders<Movie>.Filter.Eq(m => m.Id, movie.Id),
                        replacement: movie,
                        options: new ReplaceOptions { IsUpsert = true }
                    )
                    .ConfigureAwait(false);

                return movie.Id;
            }
        }

        public Task<ImdbRating> SaveImdbRating(string movieId, ImdbRating rating)
        {
            if (!ObjectId.TryParse(movieId, out var movieIdValue))
                throw NewInvalidMovieIdArgumentException(movieId);

            if (rating is null)
                throw new ArgumentNullException(nameof(rating));

            return SaveImdbRating();

            async Task<ImdbRating> SaveImdbRating()
            {
                var filter = Builders<Movie>.Filter.Eq(movie => movie.Id, new ObjectId(movieId));
                var update = Builders<Movie>.Update.Set(movie => movie.Imdb, rating);

                var movie = await _collection
                    .FindOneAndUpdateAsync
                    (
                        filter,
                        update,
                        options: new() { ReturnDocument = ReturnDocument.After }
                    )
                    .ConfigureAwait(false);

                return movie?.Imdb ?? throw NewEntityNotFoundException(nameof(SaveImdbRating), movieId);
            }
        }

        public Task<TomatoesRating> SaveTomatoesRating(string movieId, TomatoesRating rating)
        {
            if (!ObjectId.TryParse(movieId, out var movieIdValue))
                throw NewInvalidMovieIdArgumentException(movieId);

            if (rating is null)
                throw new ArgumentNullException(nameof(rating));

            return SaveTomatoesRating();

            async Task<TomatoesRating> SaveTomatoesRating()
            {
                var filter = Builders<Movie>.Filter.Eq(movie => movie.Id, new ObjectId(movieId));
                var update = Builders<Movie>.Update.Set(movie => movie.Tomatoes, rating);

                var movie = await _collection
                    .FindOneAndUpdateAsync
                    (
                        filter,
                        update,
                        options: new() { ReturnDocument = ReturnDocument.After }
                    )
                    .ConfigureAwait(false);

                return movie?.Tomatoes ?? throw NewEntityNotFoundException(nameof(SaveTomatoesRating), movieId);
            }
        }

        public async Task<int?> SaveMetacriticRating(string movieId, int? rating)
        {
            if (!ObjectId.TryParse(movieId, out var movieIdValue))
                throw NewInvalidMovieIdArgumentException(movieId);

            var filter = Builders<Movie>.Filter.Eq(movie => movie.Id, movieIdValue);
            var update = Builders<Movie>.Update.Set(movie => movie.Metacritic, rating);

            var movie = await _collection
                .FindOneAndUpdateAsync
                (
                    filter,
                    update,
                    options: new() { ReturnDocument = ReturnDocument.After }
                )
                .ConfigureAwait(false);

            return movie?.Metacritic ?? throw NewEntityNotFoundException(nameof(SaveMetacriticRating), movieId);
        }

        private static EntityNotFoundException NewEntityNotFoundException(string method, string id)
        {
            return new($"The method '{method}' failed. A movie having id '{id}' could not be found.")
            {
                EntityId = id,
                EntityName = nameof(Movie),
                EntityType = typeof(Movie).ToString()
            };
        }

        private static ArgumentException NewInvalidMovieIdArgumentException(string movieId)
        {
            return new ArgumentException($"Invalid '{nameof(movieId)}' specified."
                + $" The value '{movieId}' is an invalid MongoDB ObjectId", nameof(movieId));
        }
    }
}
