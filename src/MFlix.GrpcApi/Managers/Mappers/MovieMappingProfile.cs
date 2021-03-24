using System.Collections.Generic;
using AutoMapper;
using MFlix.Data.Movies;
using MFlix.Data.Movies.Models;
using MongoDB.Bson;

namespace MFlix.GrpcApi.Managers.Mappers
{
    public sealed class MovieMappingProfile : Profile
    {
        public MovieMappingProfile()
        {
            CreateMap<ImdbRating, Services.Imdb>();
            CreateMap<ViewerInfo, Services.Viewer>();
            CreateMap<TomatoRating, Services.Tomatoes>();
            CreateMap<Movie, Services.Movie>();
            CreateMap<Services.MovieOptions, MovieOptions>();
            CreateMap<IPagedCollection<Movie>, Services.PageInfo>();
            CreateMap<Services.MovieForSave, Movie>()
                .ForMember(
                    destination => destination.Id,
                    options => options.MapFrom(movie => DetermineMovieId(movie)))
                .ForMember(
                    destination => destination.Released,
                    options => options.MapFrom(movie => movie.Released.ToDateTime()));
        }

        private static ObjectId DetermineMovieId(Services.MovieForSave movie) =>
            ObjectId.TryParse(movie.Id, out var movieId) ? movieId : ObjectId.GenerateNewId();
    }
}
