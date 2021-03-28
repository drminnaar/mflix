using System;
using MFlix.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
