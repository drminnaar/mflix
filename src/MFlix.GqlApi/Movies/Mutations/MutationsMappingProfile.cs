using System;
using System.Globalization;
using AutoMapper;

namespace MFlix.GqlApi.Movies.Mutations
{
    public sealed class MutationsMappingProfile : Profile
    {
        public MutationsMappingProfile()
        {
            CreateMovieForSaveMap();
            CreateMap<SaveImdbInput, Services.Imdb>();
            CreateMap<Services.Imdb, SaveImdbPayload>();
            CreateTomatoesInputMap();
            CreateTomatoesPayloadMap();
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

        private void CreateTomatoesInputMap()
        {
            CreateMap<SaveTomatoesInput, Services.Tomatoes>()
                .ForMember(
                    destination => destination.Dvd,
                    options => options.MapFrom(movie => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(
                        DateTime.Parse(movie.Dvd, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)))
                )
                .ForMember(
                    destination => destination.LastUpdated,
                    options => options.MapFrom(movie => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(
                        DateTime.Parse(movie.LastUpdated, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)))
                )
                .ForMember(
                    destination => destination.Critic,
                    options => options.MapFrom(rating => new Services.Critic
                    {
                        Meter = rating.CriticMeter,
                        NumReviews = rating.CriticNumReviews,
                        Rating = rating.CriticRating
                    })
                )
                .ForMember(
                    destination => destination.Viewer,
                    options => options.MapFrom(rating => new Services.Viewer
                    {
                        Meter = rating.ViewerMeter,
                        NumReviews = rating.ViewerNumReviews,
                        Rating = rating.ViewerRating
                    })
                );
        }

        private void CreateTomatoesPayloadMap()
        {
            CreateMap<Services.Tomatoes, SaveTomatoesPayload>()
                .ForMember(
                    destination => destination.CriticMeter,
                    options => options.MapFrom(rating => rating.Critic.Meter)
                )
                .ForMember(
                    destination => destination.CriticNumReviews,
                    options => options.MapFrom(rating => rating.Critic.NumReviews)
                )
                .ForMember(
                    destination => destination.CriticRating,
                    options => options.MapFrom(rating => rating.Critic.Rating)
                )
                .ForMember(
                    destination => destination.ViewerMeter,
                    options => options.MapFrom(rating => rating.Viewer.Meter)
                )
                .ForMember(
                    destination => destination.ViewerNumReviews,
                    options => options.MapFrom(rating => rating.Viewer.NumReviews)
                )
                .ForMember(
                    destination => destination.ViewerRating,
                    options => options.MapFrom(rating => rating.Viewer.Rating)
                );
        }
    }
}
