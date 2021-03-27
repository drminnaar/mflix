using FluentValidation;
using MFlix.Services;

namespace MFlix.GrpcApi.Managers.Validators
{
    public sealed class TomatoesRatingValidator : MessageValidatorBase<Tomatoes>
    {
        public TomatoesRatingValidator()
        {
            ApplyBoxofficeRule();
            ApplyConsensusRule();
            ApplyCriticValidator();
            ApplyDvdRule();
            ApplyLastUpdatedRule();
            ApplyFreshRule();
            ApplyProductionRule();
            ApplyRottenRule();
            ApplyViewerRule();
            ApplyWebsiteRule();
        }

        private void ApplyBoxofficeRule() =>
            RuleFor(rating => rating.BoxOffice).NotNull().NotEmpty().WithMessage(rating => $"{nameof(rating.BoxOffice)} is required");

        private void ApplyConsensusRule() =>
            RuleFor(rating => rating.Consensus).NotNull().NotEmpty().WithMessage(rating => $"{nameof(rating.Consensus)} is required");

        private void ApplyCriticValidator() =>
            RuleFor(rating => rating.Critic).SetValidator(new CriticValidator());

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

        private void ApplyViewerRule() =>
            RuleFor(rating => rating.Viewer).SetValidator(new ViewerValidator());

        private void ApplyWebsiteRule() =>
            RuleFor(rating => rating.Website).NotNull().NotEmpty().WithMessage(rating => $"{nameof(rating.Website)} is required");
    }
}
