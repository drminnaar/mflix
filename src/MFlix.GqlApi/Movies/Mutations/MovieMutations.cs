using System;
using System.Threading.Tasks;
using AutoMapper;
using HotChocolate.Types;
using MFlix.GqlApi.Infrastructure;
using MFlix.Services;

namespace MFlix.GqlApi.Movies.Mutations
{
    [ExtendObjectType(AppConstants.MutationTypeName)]
    public sealed class MovieMutations
    {
        private readonly MovieService.MovieServiceClient _movieService;
        private readonly IMapper _mapper;

        public MovieMutations(MovieService.MovieServiceClient movieService, IMapper mapper)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SaveMoviePayload> SaveMovie(SaveMovieInput movie)
        {
            var result = await _movieService.SaveMovieAsync(new SaveMovieRequest
            {
                Movie = _mapper.Map<Services.MovieForSave>(movie)
            });

            return new SaveMoviePayload
            {
                MovieId = result.MovieId
            };
        }
    }
}
