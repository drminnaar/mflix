using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using MFlix.Data.Movies;
using MFlix.Data.Movies.Models;
using MFlix.DataTests.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using Xunit;

namespace MFlix.DataTests.Movies
{
    public sealed class MovieDaoTest : IClassFixture<DependencyInjectionFixture>
    {
        private readonly IServiceProvider _services;

        public MovieDaoTest(DependencyInjectionFixture setup)
        {
            if (setup is null)
                throw new ArgumentNullException(nameof(setup));

            _services = setup.Services ?? throw new ArgumentNullException(
                nameof(setup),
                "Expected dependency injection fixture to have a list of configured services.");
        }

        [Fact]
        public async Task Should_get_movie_by_id()
        {
            // arrange
            var dao = _services.GetRequiredService<IMovieDao>();

            // act
            var movie = await dao.GetMovieById("573a1390f29313caabcd4135").ConfigureAwait(true);

            // assert
            Assert.NotNull(movie);
        }

        [Fact]
        public async Task Should_return_null_when_get_movie_by_fakeid()
        {
            // arrange
            var dao = _services.GetRequiredService<IMovieDao>();

            // act
            var movie = await dao.GetMovieById("573a13c8f29313caabd77e88").ConfigureAwait(true);

            // assert
            Assert.Null(movie);
        }

        [Fact]
        public async Task Should_get_list_of_movies()
        {
            // arrange
            var dao = _services.GetRequiredService<IMovieDao>();
            var movies = new List<Movie>();
            var page = new PagedCollection<Movie>(1, 2000) as IPagedCollection<Movie>;

            // act
            for (var pageCount = 0; pageCount < 10; pageCount++)
            {
                var options = MovieOptions.Empty(
                    pageNumber: page.NextPageNumber ?? page.CurrentPageNumber,
                    pageSize: page.PageSize);

                page = await dao.GetMovies(options).ConfigureAwait(true);

                movies.AddRange(page);
            }

            // assert
            Assert.Equal(20000, movies.Count);
        }

        [Fact]
        public async Task Should_create_movie_when_saving_new_movie()
        {
            // arrange
            var dao = _services.GetRequiredService<IMovieDao>();
            var json = File.ReadAllText("TestFiles/movie.json");
            var movie = JsonSerializer
                .Deserialize<Movie>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Movie serialization failed. Expected a movie but received null instead");

            // act
            var id = await dao.SaveMovie(movie).ConfigureAwait(false);

            // assert
            Assert.NotEqual(ObjectId.Empty, id);
        }

        [Fact]
        public async Task Should_save_imdb_rating()
        {
            // arrange
            var dao = _services.GetRequiredService<IMovieDao>();
            var imdbRating = new Faker<ImdbRating>()
                .StrictMode(true)
                .RuleFor(rating => rating.Id, faker => 1502712)
                .RuleFor(rating => rating.Rating, faker => faker.Random.Double(1, 10))
                .RuleFor(rating => rating.Votes, faker => faker.Random.Int(30000, 60000))
                .Generate();
            var expected = JsonSerializer.Serialize(imdbRating);

            // act
            var movie = await dao.SaveImdbRating("573a13c8f29313caabd77e87", imdbRating).ConfigureAwait(true);
            var actual = JsonSerializer.Serialize(movie.Imdb);

            // assert
            Assert.NotNull(movie);
            Assert.Equal(expected, actual);
        }
    }
}
