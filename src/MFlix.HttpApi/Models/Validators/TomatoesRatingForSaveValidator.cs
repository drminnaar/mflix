using FluentValidation;

namespace MFlix.HttpApi.Models.Validators
{
    public sealed class TomatoesRatingForSaveValidator : AbstractValidator<TomatoesForSave>
    {
        public TomatoesRatingForSaveValidator()
        {
            ApplyBoxofficeRule();
            ApplyConsensusRule();
            ApplyCriticMeterRule();
            ApplyCriticNumReviewsRule();
            ApplyCriticRatingRule();
            ApplyDvdRule();
            ApplyLastUpdatedRule();
            ApplyFreshRule();
            ApplyProductionRule();
            ApplyRottenRule();
            ApplyViewerMeterRule();
            ApplyViewerNumReviewsRule();
            ApplyViewerRatingRule();
            ApplyWebsiteRule();
        }

        private void ApplyBoxofficeRule() =>
            RuleFor(rating => rating.BoxOffice).NotNull().NotEmpty().WithMessage(rating => $"{nameof(rating.BoxOffice)} is required");

        private void ApplyConsensusRule() =>
            RuleFor(rating => rating.Consensus).NotNull().NotEmpty().WithMessage(rating => $"{nameof(rating.Consensus)} is required");

        private void ApplyCriticMeterRule() =>
            RuleFor(critic => critic.CriticMeter).GreaterThan(0).WithMessage(critic => $"{nameof(critic.CriticMeter)} is required");

        private void ApplyCriticNumReviewsRule() =>
            RuleFor(critic => critic.CriticNumReviews).GreaterThan(0).WithMessage(critic => $"{nameof(critic.CriticNumReviews)} is required");

        private void ApplyCriticRatingRule() =>
            RuleFor(critic => critic.CriticRating).GreaterThan(0).WithMessage(critic => $"{nameof(critic.CriticRating)} is required");

        private void ApplyDvdRule() =>
            RuleFor(rating => rating.Dvd).NotNull().WithMessage(rating => $"{nameof(rating.Dvd)} is required");

        private void ApplyLastUpdatedRule() =>
            RuleFor(rating => rating.LastUpdated).NotNull().WithMessage(rating => $"{nameof(rating.LastUpdated)} is required");

        private void ApplyFreshRule() =>
            RuleFor(rating => rating.Fresh).GreaterThan(0).WithMessage(rating => $"{nameof(rating.Fresh)} is required");

        private void ApplyProductionRule() =>
            RuleFor(rating => rating.Production).NotNull().NotEmpty().WithMessage(rating => $"{nameof(rating.Production)} is required");

        private void ApplyRottenRule() =>
            RuleFor(rating => rating.Rotten).GreaterThan(0).WithMessage(rating => $"{nameof(rating.Rotten)} is required");

        private void ApplyViewerMeterRule() =>
            RuleFor(viewer => viewer.ViewerMeter).GreaterThan(0).WithMessage(viewer => $"{nameof(viewer.ViewerMeter)} is required");

        private void ApplyViewerNumReviewsRule() =>
            RuleFor(viewer => viewer.ViewerNumReviews).GreaterThan(0).WithMessage(viewer => $"{nameof(viewer.ViewerNumReviews)} is required");

        private void ApplyViewerRatingRule() =>
            RuleFor(viewer => viewer.ViewerRating).GreaterThan(0).WithMessage(viewer => $"{nameof(viewer.ViewerRating)} is required");

        private void ApplyWebsiteRule() =>
            RuleFor(rating => rating.Website).NotNull().NotEmpty().WithMessage(rating => $"{nameof(rating.Website)} is required");
    }
}
