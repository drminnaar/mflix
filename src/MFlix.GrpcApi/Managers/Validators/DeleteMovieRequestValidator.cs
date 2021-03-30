using FluentValidation;
using MFlix.Services;
using MongoDB.Bson;

namespace MFlix.GrpcApi.Managers.Validators
{
    public sealed class DeleteMovieRequestValidator : MessageValidatorBase<DeleteMovieRequest>
    {
        public DeleteMovieRequestValidator()
        {
            RuleFor(request => request.MovieId)
                .NotNull()
                .NotEmpty()
                .WithMessage(request => $"{nameof(request.MovieId)} is required");

            RuleFor(request => request.MovieId)
                .Must(movieId => ObjectId.TryParse(movieId, out var movieIdValue))
                .WithMessage(request => $"{nameof(request.MovieId)} has invalid value");
        }
    }
}
