using System;
using Grpc.Core;
using MFlix.Services;

namespace MFlix.GrpcApi.Managers.Validators
{
    public interface IMovieServiceValidator
    {
        bool IsValidImdbRatingForSave(SaveImdbRatingRequest request, out Metadata trailers);
        bool IsValidMovieForSave(SaveMovieRequest request, out Metadata trailers);
        bool IsValidTomatoesRatingForSave(SaveTomatoesRatingRequest request, out Metadata trailers);
    }

    public sealed class MovieServiceValidator : IMovieServiceValidator
    {
        private readonly MessageValidatorBase<SaveMovieRequest> _saveMovieRequestValidator;
        private readonly MessageValidatorBase<SaveImdbRatingRequest> _saveImdbRatingRequestValidator;
        private readonly MessageValidatorBase<SaveTomatoesRatingRequest> _saveTomatoesRatingRequestValidator;

        public MovieServiceValidator(
            MessageValidatorBase<SaveMovieRequest> saveMovieRequestValidator,
            MessageValidatorBase<SaveImdbRatingRequest> saveImdbRatingRequestValidator,
            MessageValidatorBase<SaveTomatoesRatingRequest> saveTomatoesRatingRequestValidator)
        {
            _saveMovieRequestValidator = saveMovieRequestValidator ?? throw new ArgumentNullException(nameof(saveMovieRequestValidator));
            _saveImdbRatingRequestValidator = saveImdbRatingRequestValidator ?? throw new ArgumentNullException(nameof(saveImdbRatingRequestValidator));
            _saveTomatoesRatingRequestValidator = saveTomatoesRatingRequestValidator ?? throw new ArgumentNullException(nameof(saveTomatoesRatingRequestValidator));
        }

        public bool IsValidMovieForSave(SaveMovieRequest request, out Metadata trailers)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return _saveMovieRequestValidator.IsValid(request, out trailers);
        }

        public bool IsValidImdbRatingForSave(SaveImdbRatingRequest request, out Metadata trailers)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return _saveImdbRatingRequestValidator.IsValid(request, out trailers);
        }

        public bool IsValidTomatoesRatingForSave(SaveTomatoesRatingRequest request, out Metadata trailers)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return _saveTomatoesRatingRequestValidator.IsValid(request, out trailers);
        }
    }
}