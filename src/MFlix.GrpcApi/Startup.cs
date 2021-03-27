using System;
using System.Reflection;
using MFlix.Data.DependencyInjection;
using MFlix.GrpcApi.Infrastructure.Interceptors;
using MFlix.GrpcApi.Managers;
using MFlix.GrpcApi.Managers.Validators;
using MFlix.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MFlix.GrpcApi
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
            services.ConfigureDataServices(_configuration);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<MessageValidatorBase<MovieForSave>, MovieForSaveValidator>();
            services.AddTransient<MessageValidatorBase<SaveImdbRatingRequest>, SaveImdbRatingRequestValidator>();
            services.AddTransient<MessageValidatorBase<SaveTomatoesRatingRequest>, SaveTomatoesRatingRequestValidator>();
            services.AddTransient<MovieService.MovieServiceBase, MovieManager>();

            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = _environment.IsDevelopment();
                options.Interceptors.Add<ServerErrorHandler>();
                options.MaxReceiveMessageSize = 1024 * 1024; // 1MB
                options.MaxSendMessageSize = 1024 * 1024; // 1MB
            });

            if (_environment.IsDevelopment())
            {
                services.AddGrpcReflection();
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                if (_environment.IsDevelopment())
                {
                    endpoints.MapGrpcReflectionService();
                }

                endpoints.MapGrpcService<MovieManager>();

                endpoints.MapGet("/", async context =>
                {
                    await context
                        .Response
                        .WriteAsync(EndpointMappingErrorMessage)
                        .ConfigureAwait(true);
                });
            });
        }

        private static string EndpointMappingErrorMessage =>
            "Communication with gRPC endpoints must be made through a gRPC client."
            + " To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909";
    }
}
