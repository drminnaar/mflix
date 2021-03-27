using FluentValidation;
using MFlix.Services;
using MongoDB.Bson;

namespace MFlix.GrpcApi.Managers.Validators
{
    public sealed class SaveImdbRatingRequestValidator : MessageValidatorBase<SaveImdbRatingRequest>
    {
        public SaveImdbRatingRequestValidator() : base()
        {
            ApplyMovieRule();
            ApplyMovieIdRule();
        }

        private void ApplyMovieRule()
        {
            RuleFor(request => request.Imdb).SetValidator(new ImdbRatingForSaveValidator());
        }

        private void ApplyMovieIdRule()
        {
            RuleFor(request => request.MovieId).NotNull().NotEmpty().WithMessage("Movie id is required");
            RuleFor(request => request.MovieId).Must(movieId => ObjectId.TryParse(movieId, out var movieIdValue)).WithMessage("Invalid movie id specified");
        }
    }
}
