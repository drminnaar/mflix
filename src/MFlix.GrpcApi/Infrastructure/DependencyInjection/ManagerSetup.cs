using MFlix.GrpcApi.Managers;
using MFlix.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MFlix.GrpcApi.Infrastructure.DependencyInjection
{
    public static class ManagerSetup
    {
        public static IServiceCollection ConfigureManagers(this IServiceCollection services)
        {
            services.AddTransient<MovieService.MovieServiceBase, MovieManager>();
            return services;
        }
    }
}
