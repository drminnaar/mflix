using System;
using System.Threading.Tasks;
using AutoMapper;
using HotChocolate;
using HotChocolate.Types;
using MFlix.GqlApi.Infrastructure;
using MFlix.GqlApi.Movies.Mutations.Models;
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

        [GraphQLDescription("Save IMDB rating information")]
        public async Task<SaveImdbPayload> SaveImdb(SaveImdbInput imdb)
        {
            if (imdb is null)
                throw new ArgumentNullException(nameof(imdb));

            var result = await _movieService.SaveImdbRatingAsync(new SaveImdbRatingRequest
            {
                Imdb = _mapper.Map<Imdb>(imdb),
                MovieId = imdb.MovieId
            });

            return _mapper.Map<SaveImdbPayload>(result.Imdb);
        }

        [GraphQLDescription("Save movie information")]
        public async Task<SaveMoviePayload> SaveMovie(SaveMovieInput movie)
        {
            if (movie is null)
                throw new ArgumentNullException(nameof(movie));

            var result = await _movieService.SaveMovieAsync(new SaveMovieRequest
            {
                Movie = _mapper.Map<Services.MovieForSave>(movie)
            });

            return new SaveMoviePayload
            {
                MovieId = result.MovieId
            };
        }

        [GraphQLDescription("Save tomatoes rating information")]
        public async Task<SaveTomatoesPayload> SaveTomatoes(SaveTomatoesInput tomatoes)
        {
            if (tomatoes is null)
                throw new ArgumentNullException(nameof(tomatoes));

            var result = await _movieService.SaveTomatoesRatingAsync(new SaveTomatoesRatingRequest
            {
                MovieId = tomatoes.MovieId,
                Tomatoes = _mapper.Map<Tomatoes>(tomatoes)
            });

            return _mapper.Map<SaveTomatoesPayload>(result.Tomatoes);
        }
    }
}
