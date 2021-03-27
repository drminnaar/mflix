using MFlix.Services;

namespace MFlix.GrpcApi.Managers.Validators
{
    public sealed class SaveMovieRequestValidator : MessageValidatorBase<SaveMovieRequest>
    {
        public SaveMovieRequestValidator() : base()
        {
            RuleFor(request => request.Movie).SetValidator(new MovieForSaveValidator());
        }
    }
}
