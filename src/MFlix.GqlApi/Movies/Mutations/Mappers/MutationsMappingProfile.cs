using System;
using System.Globalization;
using AutoMapper;

namespace MFlix.GqlApi.Movies.Mutations.Mappers
{
    public sealed class MutationsMappingProfile : Profile
    {
        public MutationsMappingProfile()
        {
            CreateMovieForSaveMap();
            CreateMap<SaveImdbInput, Services.Imdb>();
            CreateMap<Services.Imdb, SaveImdbPayload>();
        }

        private void CreateMovieForSaveMap()
        {
            CreateMap<SaveMovieInput, Services.MovieForSave>()
                .ForMember(
                    destination => destination.Id,
                    options => options.MapFrom(_ => _.Id ?? string.Empty)
                )
                .ForMember(
                    destination => destination.Released,
                    options => options.MapFrom(movie => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(
                        DateTime.Parse(movie.Released, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)))
                );
        }
    }
}
