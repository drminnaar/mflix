using FluentValidation;
using MFlix.Services;

namespace MFlix.GrpcApi.Managers.Validators
{
    public sealed class ImdbRatingForSaveValidator : MessageValidatorBase<Imdb>
    {
        public ImdbRatingForSaveValidator() : base()
        {
            ApplyIdRule();
            ApplyRatingRule();
            ApplyVotesRule();
        }

        private void ApplyIdRule() =>
            RuleFor(r => r.Id).GreaterThan(0).WithMessage("Id is required");

        private void ApplyRatingRule() =>
            RuleFor(r => r.Rating).GreaterThan(0).WithMessage("Rating is required");

        private void ApplyVotesRule() =>
            RuleFor(r => r.Votes).GreaterThan(0).WithMessage("Votes is required");
    }
}
