using System;
using MFlix.GqlApi.Infrastructure;
using MFlix.GqlApi.Infrastructure.Configuration;
using MFlix.GqlApi.Movies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MFlix.GqlApi
{
    public sealed class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpcClient<Services.MovieService.MovieServiceClient>(options =>
            {
                var address = _configuration
                    .GetSection(GrpcClientSettings.ConfigurationSectionName)
                    .Get<GrpcClientSettings>()
                    .Address;

                options.Address = new Uri(address);
            });

            services
                .AddGraphQLServer()
                .AddQueryType(qt => qt.Name(AppConstants.QueryTypeName))
                .AddTypeExtension<MovieQueries>()
                .AddType<MovieType>()
                .AddType<MovieListType>()
                .AddType<MovieOptionsType>()
                .AddType<PageInfoType>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
