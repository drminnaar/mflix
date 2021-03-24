using System.Collections.Generic;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using MFlix.Data.Movies;
using MFlix.Data.Movies.Models;
using MongoDB.Bson;

namespace MFlix.GrpcApi.Managers.Mappers
{
    public sealed class MovieMappingProfile : Profile
    {
        public MovieMappingProfile()
        {
            CreateMap<ImdbRating, Services.Imdb>().ReverseMap();
            CreateMap<ViewerInfo, Services.Viewer>().ReverseMap();
            CreateMap<CriticInfo, Services.Critic>().ReverseMap();
            CreateMap<TomatoesRating, Services.Tomatoes>()
                .ForMember(
                    destination => destination.LastUpdated,
                    options => options.MapFrom(rating => Timestamp.FromDateTime(rating.LastUpdated)))
                .ForMember(
                    destination => destination.Dvd,
                    options => options.MapFrom(rating => rating.Dvd.HasValue ? Timestamp.FromDateTime(rating.Dvd.Value) : null));
            CreateMap<Services.Tomatoes, TomatoesRating>()
                .ForMember(
                    destination => destination.LastUpdated,
                    options => options.MapFrom(rating => rating.LastUpdated.ToDateTime()))
                .ForMember(
                    destination => destination.Dvd,
                    options => options.MapFrom(rating => rating.Dvd.ToDateTime().Date));
            CreateMap<Movie, Services.Movie>();
            CreateMap<Services.MovieOptions, MovieOptions>();
            CreateMap<IPagedCollection<Movie>, Services.PageInfo>();
            CreateMap<Services.MovieForSave, Movie>()
                .ForMember(
                    destination => destination.Id,
                    options => options.MapFrom(movie => DetermineMovieId(movie)))
                .ForMember(
                    destination => destination.Released,
                    options => options.MapFrom(movie => movie.Released.ToDateTime()))
                .ForMember(
                    destination => destination.Type,
                    options => options.MapFrom(movie => "movie"));
        }

        private static ObjectId DetermineMovieId(Services.MovieForSave movie) =>
            ObjectId.TryParse(movie.Id, out var movieId) ? movieId : ObjectId.GenerateNewId();
    }
}
