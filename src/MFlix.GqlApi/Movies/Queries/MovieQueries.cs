using System;
using System.Threading.Tasks;
using AutoMapper;
using HotChocolate.Types;
using MFlix.GqlApi.Infrastructure;
using MFlix.GqlApi.Movies.Models;

namespace MFlix.GqlApi.Movies.Queries
{
    [ExtendObjectType(AppConstants.QueryTypeName)]
    public sealed class MovieQueries
    {
        private readonly Services.MovieService.MovieServiceClient _movieService;
        private readonly IMapper _mapper;

        public MovieQueries(Services.MovieService.MovieServiceClient movieService, IMapper mapper)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Movie?> GetMovieById(string movieId)
        {
            var response = await _movieService.GetMovieByIdAsync(new Services.GetMovieByIdRequest
            {
                MovieId = movieId
            });

            return response.Movie is null ? null : _mapper.Map<Movie>(response.Movie);
        }

        public async Task<MovieList> GetMovies(MovieOptions? options)
        {
            if (options is null)
                options = MovieOptions.Default;

            var response = await _movieService.GetMovieListAsync(new Services.GetMovieListRequest
            {
                Options = _mapper.Map<Services.MovieOptions>(options)
            });

            return _mapper.Map<MovieList>(response);
        }
    }
}
