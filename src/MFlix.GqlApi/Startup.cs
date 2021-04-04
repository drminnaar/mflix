using System;
using System.Reflection;
using MFlix.GqlApi.Infrastructure;
using MFlix.GqlApi.Infrastructure.Configuration;
using MFlix.GqlApi.Movies.Mutations;
using MFlix.GqlApi.Movies.Queries;
using MFlix.GqlApi.Movies.Queries.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

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
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

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
                .AddMutationType(mt => mt.Name(AppConstants.MutationTypeName))
                    .AddTypeExtension<MovieMutations>()
                .AddQueryType(qt => qt.Name(AppConstants.QueryTypeName))
                    .AddTypeExtension<MovieQueries>()
                .AddType<MovieOptionsType>()
                .AddType<MovieType>()
                .AddType<MovieListType>()
                .AddType<PageInfoType>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
