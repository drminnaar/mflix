using System;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using MFlix.Data.Movies;
using MFlix.Services;

namespace MFlix.GrpcApi.Managers
{
    public sealed class MovieManager : MovieService.MovieServiceBase
    {
        private readonly IMovieDao _movieDao;
        private readonly IMapper _mapper;

        public MovieManager(IMovieDao movieDao, IMapper mapper)
        {
            _movieDao = movieDao ?? throw new ArgumentNullException(nameof(movieDao));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<GetMovieByIdResponse> GetMovieById(GetMovieByIdRequest request, ServerCallContext context)
        {
            var movie = await _movieDao
                .GetMovieById(request.MovieId)
                .ConfigureAwait(true);

            return new GetMovieByIdResponse
            {
                Movie = _mapper.Map<Movie>(movie)
            };
        }
    }
}
