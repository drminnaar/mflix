using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace MFlix.HttpApi.Infrastructure.DependencyInjection
{
    internal static class SwaggerSetup
    {
        internal static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "MFlix Api - V1",
                        Version = "v1",
                        Description = "A sample HTTP Api (guided by REST principles) to manage movie data",
                        TermsOfService = new Uri("http://example.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "Developers",
                            Email = "developers@example.com"
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Apache 2.0",
                            Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html")
                        }
                    });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setup.EnableAnnotations();
                setup.IncludeXmlComments(xmlPath);
            });
        }

        internal static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "MFlix Api V1");
                setup.RoutePrefix = "swagger";
                setup.DefaultModelExpandDepth(2);
                setup.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
                setup.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
                setup.EnableDeepLinking();
            });
            return app;
        }
    }
}