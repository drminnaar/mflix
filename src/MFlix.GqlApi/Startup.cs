using System;
using System.Reflection;
using MFlix.GqlApi.Infrastructure;
using MFlix.GqlApi.Infrastructure.Configuration;
using MFlix.GqlApi.Infrastructure.Filters;
using MFlix.GqlApi.Movies.Mutations;
using MFlix.GqlApi.Movies.Queries;
using MFlix.GqlApi.Movies.Queries.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Extensions.Logging;

namespace MFlix.GqlApi
{
    public sealed class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
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
                .AddErrorFilter(provider =>
                {
                    return new ServerErrorFilter(
                        logger: new SerilogLoggerProvider(Log.Logger).CreateLogger(nameof(ServerErrorFilter)),
                        environment: _environment);
                })
                .AddMutationType(mt => mt.Name(AppConstants.MutationTypeName))
                    .AddTypeExtension<MovieMutations>()
                .AddQueryType(qt => qt.Name(AppConstants.QueryTypeName))
                    .AddTypeExtension<MovieQueries>()
                .AddType<MovieOptionsType>()
                .AddType<MovieType>()
                .AddType<MovieListType>()
                .AddType<PageInfoType>()
                .ModifyRequestOptions(options => options.IncludeExceptionDetails = _environment.IsDevelopment());
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
