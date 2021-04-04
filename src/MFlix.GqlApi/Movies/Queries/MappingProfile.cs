using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using MFlix.GqlApi.Movies.Queries.Models;

namespace MFlix.GqlApi.Movies.Queries
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Services.PageInfo, PageInfo>();
            CreateMap<Services.Critic, Critic>();
            CreateMap<Services.Viewer, Viewer>();
            CreateTomatoesRatingMap();
            CreateMap<Services.Imdb, ImdbRating>();
            CreateMap<Services.Movie, Movie>();
            CreateMap<MovieOptions, Services.MovieOptions>();
            CreateMovieOptionsMap();
            CreateGetMovieListResponseMap();
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

        private void CreateGetMovieListResponseMap()
        {
            CreateMap<Services.GetMovieListResponse, MovieList>()
                .ForMember(
                    destination => destination.Movies,
                    options => options.MapFrom(response => response.Movies)
                )
                .ForMember(
                    destination => destination.PageInfo,
                    options => options.MapFrom(response => response.PageInfo)
                );
        }

        private void CreateMovieOptionsMap()
        {
            const int DefaultPageNumber = 1;
            const int DefaultPageSize = 10;


            CreateMap<MovieOptions, Services.MovieOptions>()
                .ForMember(
                    destination => destination.PageNumber,
                    options => options.MapFrom(_ => _.Page ?? DefaultPageNumber)
                )
                .ForMember(
                    destination => destination.PageSize,
                    options => options.MapFrom(_ => _.Limit ?? DefaultPageSize)
                )
                .ForMember(
                    destination => destination.Rated,
                    options => options.MapFrom(_ => _.Rated ?? string.Empty)
                )
                .ForMember(
                    destination => destination.Runtime,
                    options => options.MapFrom(_ => _.Runtime ?? string.Empty)
                )
                .ForMember(
                    destination => destination.Title,
                    options => options.MapFrom(_ => _.Title ?? string.Empty)
                )
                .ForMember(
                    destination => destination.Type,
                    options => options.MapFrom(_ => _.Type ?? string.Empty)
                )
                .ForMember(
                    destination => destination.Year,
                    options => options.MapFrom(_ => _.Year ?? string.Empty)
                )
                .AfterMap((options, response, context) =>
                {
                    var cast = options.Cast?.FromCsv() ?? Array.Empty<string>();
                    response.Cast.Clear();
                    response.Cast.AddRange(cast);

                    var directors = options.Directors?.FromCsv() ?? Array.Empty<string>();
                    response.Directors.Clear();
                    response.Directors.AddRange(directors);

                    var genres = options.Genres?.FromCsv() ?? Array.Empty<string>();
                    response.Genres.Clear();
                    response.Genres.AddRange(genres);

                    var order = options.Order?.FromCsv() ?? new List<string> { "title" };
                    response.SortBy.Clear();
                    response.SortBy.AddRange(order);
                });
        }
    }
}
