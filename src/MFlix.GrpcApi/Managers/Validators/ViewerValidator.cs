using FluentValidation;
using MFlix.Services;

namespace MFlix.GrpcApi.Managers.Validators
{
    public sealed class ViewerValidator : MessageValidatorBase<Viewer>
    {
        public ViewerValidator() : base()
        {
            ApplyMeterRule();
            ApplyNumReviewsRule();
            ApplyRatingRule();
        }

        private void ApplyMeterRule() =>
            RuleFor(viewer => viewer.Meter).GreaterThan(0).WithMessage(viewer => $"{nameof(viewer.Meter)} is required");

        private void ApplyNumReviewsRule() =>
            RuleFor(viewer => viewer.NumReviews).GreaterThan(0).WithMessage(viewer => $"{nameof(viewer.NumReviews)} is required");

        private void ApplyRatingRule() =>
            RuleFor(viewer => viewer.Rating).GreaterThan(0).WithMessage(viewer => $"{nameof(viewer.Rating)} is required");
    }
}
