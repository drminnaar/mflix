using MFlix.GrpcApi.Managers.Validators;
using MFlix.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MFlix.GrpcApi.Infrastructure.DependencyInjection
{
    public static class ValidatorSetup
    {
        public static IServiceCollection ConfigureValidators(this IServiceCollection services)
        {
            services.AddTransient<MessageValidatorBase<SaveMovieRequest>, SaveMovieRequestValidator>();
            services.AddTransient<MessageValidatorBase<SaveImdbRatingRequest>, SaveImdbRatingRequestValidator>();
            services.AddTransient<MessageValidatorBase<SaveTomatoesRatingRequest>, SaveTomatoesRatingRequestValidator>();
            services.AddTransient<IMovieServiceValidator, MovieServiceValidator>();
            return services;
        }
    }
}
