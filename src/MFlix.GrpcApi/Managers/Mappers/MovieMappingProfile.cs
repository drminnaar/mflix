﻿using AutoMapper;
using MFlix.Data.Movies.Models;

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
        }
    }
}
