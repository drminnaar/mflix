using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using MFlix.Data.Movies;
using MFlix.Data.Movies.Models;
using MFlix.GrpcApi.Managers.Validators;

namespace MFlix.GrpcApi.Managers
{
    public sealed class MovieManager : Services.MovieService.MovieServiceBase
    {
        private readonly IMovieDao _movieDao;
        private readonly IMapper _mapper;
        private readonly IMovieServiceValidator _validator;

        public MovieManager(
            IMovieDao movieDao,
            IMapper mapper,
            IMovieServiceValidator validator)
        {
            _movieDao = movieDao ?? throw new ArgumentNullException(nameof(movieDao));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public override async Task<Services.DeleteMovieResponse> DeleteMovie(Services.DeleteMovieRequest request, ServerCallContext context)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            if (!_validator.IsValidDeleteMovieRequest(request, out var trailers))
                throw NewInvalidArgumentRpcException("Invalid movie id", trailers);

            var movie = await _movieDao
                .DeleteMovie(request.MovieId)
                .ConfigureAwait(true);

            return new Services.DeleteMovieResponse
            {
                MovieId = movie.Id.ToString()
            };
        }

        public override async Task<Services.GetMovieByIdResponse> GetMovieById(
            Services.GetMovieByIdRequest request,
            ServerCallContext context)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            if (!_validator.IsValidGetMovieByIdRequest(request, out var trailers))
                throw NewInvalidArgumentRpcException("Invalid movie id", trailers);

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
            if (request is null) throw new ArgumentNullException(nameof(request));

            if (!_validator.IsValidMovieForSave(request, out var trailers))
                throw NewInvalidArgumentRpcException("Invalid movie", trailers);

            var movie = _mapper.Map<Movie>(request.Movie);

            var movieId = await _movieDao
                .SaveMovie(movie)
                .ConfigureAwait(true);

            return new Services.SaveMovieResponse
            {
                MovieId = movieId.ToString()
            };
        }

        public override async Task<Services.SaveImdbRatingResponse> SaveImdbRating(Services.SaveImdbRatingRequest request, ServerCallContext context)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            if (!_validator.IsValidImdbRatingForSave(request, out var trailers))
                throw NewInvalidArgumentRpcException("Invalid Imdb details", trailers);

            var imdbRating = _mapper.Map<ImdbRating>(request.Imdb);

            var imdbRatingFromSave = await _movieDao
                .SaveImdbRating(request.MovieId, imdbRating)
                .ConfigureAwait(true);

            return new Services.SaveImdbRatingResponse
            {
                Imdb = _mapper.Map<Services.Imdb>(imdbRatingFromSave)
            };
        }

        public override async Task<Services.SaveTomatoesRatingResponse> SaveTomatoesRating(Services.SaveTomatoesRatingRequest request, ServerCallContext context)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            if (!_validator.IsValidTomatoesRatingForSave(request, out var trailers))
                throw NewInvalidArgumentRpcException("Invalid tomatoes details", trailers);

            var tomatoesRating = _mapper.Map<TomatoesRating>(request.Tomatoes);

            var tomatoesRatingFromSave = await _movieDao
                .SaveTomatoesRating(request.MovieId, tomatoesRating)
                .ConfigureAwait(true);

            return new Services.SaveTomatoesRatingResponse
            {
                Tomatoes = _mapper.Map<Services.Tomatoes>(tomatoesRatingFromSave)
            };
        }

        public override async Task<Services.SaveMetacriticRatingResponse> SaveMetacriticRating(Services.SaveMetacriticRatingRequest request, ServerCallContext context)
        {
            var metacriticRating = await _movieDao
                .SaveMetacriticRating(request.MovieId, request.MetacriticRating)
                .ConfigureAwait(true);

            return new Services.SaveMetacriticRatingResponse
            {
                MetacriticRating = metacriticRating
            };
        }

        private static RpcException NewInvalidArgumentRpcException(string message, Metadata trailers) =>
            new(new Status(StatusCode.InvalidArgument, message), trailers);
    }
}
