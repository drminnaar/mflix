﻿using System;
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
        Task<Movie> SaveImdbRating(string movieId, ImdbRating rating);
        Task<ObjectId> SaveMovie(Movie movie);
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
            if (string.IsNullOrWhiteSpace(movieId))
                throw new ArgumentException($"'{nameof(movieId)}' cannot be null or whitespace.", nameof(movieId));

            return DeleteMovie();

            async Task<Movie> DeleteMovie()
            {
                var filter = Builders<Movie>.Filter.Eq(movie => movie.Id, new ObjectId(movieId));

                return await _collection
                    .FindOneAndDeleteAsync(filter)
                    .ConfigureAwait(false);
            };
        }

        public Task<Movie> GetMovieById(string movieId) => GetMovieById(ObjectId.Parse(movieId));

        public Task<Movie> GetMovieById(ObjectId movieId) => _collection
                .Find(movie => movie.Id == movieId)
                .FirstOrDefaultAsync();

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
                var result = await _collection
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

        public Task<Movie> SaveImdbRating(string movieId, ImdbRating rating)
        {
            if (rating is null)
                throw new ArgumentNullException(nameof(rating));

            return SaveImdbRating();

            async Task<Movie> SaveImdbRating()
            {
                var filter = Builders<Movie>.Filter.Eq(movie => movie.Id, new ObjectId(movieId));
                var update = Builders<Movie>.Update.Set(movie => movie.Imdb, rating);

                return await _collection
                    .FindOneAndUpdateAsync
                    (
                        filter,
                        update,
                        options: new() { ReturnDocument = ReturnDocument.After }
                    )
                    .ConfigureAwait(false);
            }
        }
    }
}
