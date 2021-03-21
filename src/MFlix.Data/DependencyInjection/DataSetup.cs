using System;
using MFlix.Data.Configuration;
using MFlix.Data.Movies;
using MFlix.Data.Movies.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MFlix.Data.DependencyInjection
{
    public static class DataSetup
    {
        private const string DbName = "mflix";
        private const string MovieCollectionName = "movies";

        public static IServiceCollection ConfigureDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            ConfigureClassMappings();

            return services
                .ConfigureConfigurationServices(configuration)
                .ConfigureMongoDBServices()
                .ConfigureDataAccessServices();
        }

        private static IServiceCollection ConfigureConfigurationServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<MongoAppSettings>(configuration.GetSection(MongoAppSettings.ConfigurationSectionName))
                .AddSingleton(provider => provider.GetRequiredService<IOptions<MongoAppSettings>>().Value);
        }

        private static IServiceCollection ConfigureMongoDBServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IMongoClient>(provider => new MongoClient(provider.GetRequiredService<MongoAppSettings>().ConnectionString))
                .AddTransient(provider => provider.GetRequiredService<IMongoClient>().GetDatabase(name: DbName))
                .AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<Movie>(name: MovieCollectionName));
        }

        private static IServiceCollection ConfigureDataAccessServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IMovieFilterBuilder, MovieFilterBuilder>()
                .AddTransient<IMovieSortDefinition, MovieSortDefinition>()
                .AddTransient<IMovieDao, MovieDao>();
        }

        private static void ConfigureClassMappings()
        {
            BsonClassMap.RegisterClassMap<Movie>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<ImdbRating>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<TomatoRating>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
