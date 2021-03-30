using System;
using System.Net.Mime;
using System.Reflection;
using MFlix.HttpApi.Infrastructure.DependencyInjection;
using MFlix.HttpApi.Infrastructure.Filters;
using MFlix.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace MFlix.HttpApi
{
    public sealed class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddGrpcClient<MovieService.MovieServiceClient>(options =>
            {
                options.Address = new Uri("https://localhost:5001");
            });
            services
                .AddControllers(options =>
                {
                    // Custom error handling filter
                    options.Filters.Add<ServerErrorFilter>();

                    options.ReturnHttpNotAcceptable = true;
                    options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
                })
                .AddXmlDataContractSerializerFormatters()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problem = new ValidationProblemDetails(context.ModelState)
                        {
                            Detail = string.Empty,
                            Instance = context.HttpContext.Request?.Path.Value ?? string.Empty,
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Title = $"There are {context.ModelState.ErrorCount} validation errors",
                            Type = $"https://httpstatuses.com/{StatusCodes.Status422UnprocessableEntity}"
                        };
                        problem.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                        var result = new ObjectResult(problem);
                        result.ContentTypes.Add(MediaTypeNames.Application.Json);
                        result.ContentTypes.Add(MediaTypeNames.Application.Xml);
                        return result;
                    };
                });
            services.ConfigureSwagger();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();

            // Custom error handling middleware (no xml support)
            // app.UseMiddleware<ServerErrorHandler>();

            // Use builtin exception handler with custom errors controller
            //app.UseExceptionHandler($"/{ErrorsController.ErrorsPath}");
            app.UseStaticFiles();
            app.UseCustomSwagger();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
