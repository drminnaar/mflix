using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.Types;

namespace MFlix.GqlApi.Movies
{
    [ExtendObjectType("Query")]
    public sealed class MovieQueries
    {
        private readonly Services.MovieService.MovieServiceClient _movieService;

        public MovieQueries(Services.MovieService.MovieServiceClient movieService)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
        }

        public async Task<Movie?> GetMovieById(string movieId)
        {
            var response = await _movieService.GetMovieByIdAsync(new Services.GetMovieByIdRequest
            {
                MovieId = movieId
            });

            return response.Movie is null
                ? null
                : new Movie { Id = response.Movie.Id, Title = response.Movie.Title };
        }

        public async Task<MovieList> ListMovies(MovieOptions options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            var serviceOptions = new Services.MovieOptions
            {
                PageNumber = options.PageNumber ?? 1,
                PageSize = options.PageSize ?? 10,
                Rated = options.Rated ?? string.Empty,
                Runtime = options.Runtime ?? string.Empty,
                Title = options.Title ?? string.Empty,
                Type = options.Type ?? string.Empty,
                Year = options.Year ?? string.Empty
            };
            serviceOptions.Cast.AddRange(options.Cast?.FromCsv());
            serviceOptions.Directors.AddRange(options.Directors?.FromCsv() ?? Array.Empty<string>());
            serviceOptions.Genres.AddRange(options.Genres?.FromCsv() ?? Array.Empty<string>());
            serviceOptions.SortBy.AddRange(options.OrderBy?.FromCsv() ?? new List<string> { "title" });

            var response = await _movieService.GetMovieListAsync(new Services.GetMovieListRequest
            {
                Options = serviceOptions
            });

            return new MovieList
            {
                Movies = response
                    .Movies
                    .Select(movie => new Movie { Id = movie.Title, Title = movie.Title })
                    .ToList(),
                PageInfo = new PageInfo
                {
                    CurrentPageNumber = response.PageInfo.CurrentPageNumber,
                    HasNext = response.PageInfo.HasNext,
                    HasPrevious = response.PageInfo.HasPrevious,
                    ItemCount = response.PageInfo.ItemCount,
                    LastPageNumber = response.PageInfo.LastPageNumber,
                    NextPageNumber = response.PageInfo.NextPageNumber,
                    PageCount = response.PageInfo.PageCount,
                    PageSize = response.PageInfo.PageSize,
                    PreviousPageNumber = response.PageInfo.PreviousPageNumber
                }
            };
        }
    }
}
