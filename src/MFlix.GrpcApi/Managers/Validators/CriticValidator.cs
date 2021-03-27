using FluentValidation;
using MFlix.Services;

namespace MFlix.GrpcApi.Managers.Validators
{
    public sealed class CriticValidator : MessageValidatorBase<Critic>
    {
        public CriticValidator() : base()
        {
            ApplyMeterRule();
            ApplyNumReviewsRule();
            ApplyRatingRule();
        }

        private void ApplyMeterRule() =>
            RuleFor(critic => critic.Meter).GreaterThan(0).WithMessage(critic => $"{nameof(critic.Meter)} is required");

        private void ApplyNumReviewsRule() =>
            RuleFor(critic => critic.NumReviews).GreaterThan(0).WithMessage(critic => $"{nameof(critic.NumReviews)} is required");

        private void ApplyRatingRule() =>
            RuleFor(critic => critic.Rating).GreaterThan(0).WithMessage(critic => $"{nameof(critic.Rating)} is required");
    }
}
