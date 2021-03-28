using System;
using MFlix.HttpApi.Controllers;
using MFlix.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace MFlix.HttpApi
{
    public sealed class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpcClient<MovieService.MovieServiceClient>(options =>
            {
                options.Address = new Uri("https://localhost:5001");
            });
            services
                .AddControllers(options =>
                {
                    options.ReturnHttpNotAcceptable = true;
                    options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
                })
                .AddXmlDataContractSerializerFormatters();
        }

        public void Configure(IApplicationBuilder app)
        {
            // uncomment below to use error handling middleware instead
            // but doesn't support xml
            // app.UseMiddleware<ServerErrorHandler>();
            app.UseExceptionHandler($"/{ErrorsController.ErrorsPath}");
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
