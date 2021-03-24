using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using MFlix.Data.Movies;
using MFlix.Data.Movies.Models;

namespace MFlix.GrpcApi.Managers
{
    public sealed class MovieManager : Services.MovieService.MovieServiceBase
    {
        private readonly IMovieDao _movieDao;
        private readonly IMapper _mapper;

        public MovieManager(IMovieDao movieDao, IMapper mapper)
        {
            _movieDao = movieDao ?? throw new ArgumentNullException(nameof(movieDao));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<Services.GetMovieByIdResponse> GetMovieById(
            Services.GetMovieByIdRequest request,
            ServerCallContext context)
        {
            var movie = await _movieDao
                .GetMovieById(request.MovieId)
                .ConfigureAwait(true);

            return new Services.GetMovieByIdResponse
            {
                Movie = _mapper.Map<Services.Movie>(movie)
            };
        }

        public override async Task<Services.GetMovieListResponse> GetMovieList(
            Services.GetMovieListRequest request,
            ServerCallContext context)
        {
            var options = _mapper.Map<MovieOptions>(request.Options);

            var movies = await _movieDao
                .GetMovies(options)
                .ConfigureAwait(true)
                ?? new PagedCollection<Movie>();

            var response = new Services.GetMovieListResponse
            {
                PageInfo = _mapper.Map<Services.PageInfo>(movies)
            };
            response.Movies.AddRange(_mapper.Map<List<Services.Movie>>(movies));

            return response;
        }

        public override async Task<Services.SaveMovieResponse> SaveMovie(Services.SaveMovieRequest request, ServerCallContext context)
        {
            var movie = _mapper.Map<Movie>(request.Movie);

            var movieId = await _movieDao
                .SaveMovie(movie)
                .ConfigureAwait(true);

            return new Services.SaveMovieResponse
            {
                MovieId = movieId.ToString()
            };
        }
    }
}
