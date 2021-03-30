using FluentValidation;

namespace MFlix.HttpApi.Models.Validators
{
    public sealed class MovieForSaveValidator : AbstractValidator<MovieForSave>
    {
        public MovieForSaveValidator()
        {
            ApplyCastRule();
            ApplyDirectorsRule();
            ApplyGenresRule();
            ApplyPlotRule();
            ApplyRatedRule();
            ApplyReleasedRule();
            ApplyRuntimeRule();
            ApplyTitleRule();
            ApplyYearRule();
        }

        private void ApplyCastRule()
        {
            RuleFor(m => m.Cast).NotNull().NotEmpty().WithMessage("At least 1 cast member is required");
            RuleForEach(movie => movie.Cast).NotEmpty().WithMessage("Please specify a valid cast member for the property '{PropertyName}' at position {CollectionIndex}");
        }

        private void ApplyDirectorsRule()
        {
            RuleFor(m => m.Directors).NotNull().NotEmpty().WithMessage("At least 1 director is required");
            RuleForEach(movie => movie.Directors).NotEmpty().WithMessage("Please specify a valid director for the property '{PropertyName}' at position {CollectionIndex}");
        }

        private void ApplyGenresRule()
        {
            RuleFor(m => m.Genres).NotNull().NotEmpty().WithMessage("At least 1 genre is required");
            RuleForEach(movie => movie.Genres).NotEmpty().WithMessage("Please specify a valid genre for the property '{PropertyName}' at position {CollectionIndex}");
        }

        private void ApplyPlotRule() =>
            RuleFor(m => m.Plot).NotNull().NotEmpty().WithMessage("Plot is required");

        private void ApplyRatedRule() =>
            RuleFor(m => m.Rated).NotNull().NotEmpty().WithMessage("Rated is required");

        private void ApplyReleasedRule() =>
            RuleFor(m => m.Released).NotNull().NotEmpty().WithMessage("Released is required");

        private void ApplyRuntimeRule() =>
            RuleFor(m => m.Runtime).NotNull().NotEmpty().WithMessage("Runtime is required");

        private void ApplyTitleRule() =>
            RuleFor(m => m.Title).NotNull().NotEmpty().WithMessage("Title is required");

        private void ApplyYearRule() =>
            RuleFor(m => m.Year).NotNull().NotEmpty().WithMessage("Year is required");
    }
}
