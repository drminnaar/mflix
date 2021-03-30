using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;

namespace MFlix.HttpApi.Models.Mappers
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Services.Critic, Critic>();
            CreateMap<Services.Viewer, Viewer>();
            CreateTomatoesRatingMap();
            CreateMap<Services.Imdb, ImdbRating>();
            CreateMovieMap();
            CreateMap<MovieOptions, Services.MovieOptions>();
        }

        private void CreateTomatoesRatingMap()
        {
            CreateMap<Services.Tomatoes, TomatoesRating>()
                .ForMember(
                    destination => destination.Dvd,
                    options => options.MapFrom(rating => rating.Dvd.ToDateTime().Date.ToString(CultureInfo.InvariantCulture))
                )
                .ForMember(
                    destination => destination.LastUpdated,
                    options => options.MapFrom(rating => rating.LastUpdated.ToDateTime().ToString(CultureInfo.InvariantCulture))
                );
        }

        private void CreateMovieMap()
        {
            CreateMap<Services.Movie, Movie>()
                .ForMember(
                    destination => destination.CastMembers,
                    options => options.MapFrom(movie => new List<Cast>(movie.Cast.Select(cast => new Cast(cast))))
                )
                .ForMember(
                    destination => destination.Directors,
                    options => options.MapFrom(movie => new List<Director>(movie.Directors.Select(director => new Director(director))))
                )
                .ForMember(
                    destination => destination.Genres,
                    options => options.MapFrom(movie => new List<Genre>(movie.Genres.Select(genre => new Genre(genre))))
                );
        }
    }
}
