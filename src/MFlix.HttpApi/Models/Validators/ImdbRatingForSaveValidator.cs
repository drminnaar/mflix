using FluentValidation;

namespace MFlix.HttpApi.Models.Validators
{
    public sealed class ImdbRatingForSaveValidator : AbstractValidator<ImdbForSave>
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
