using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace MFlix.HttpApi.Models
{
    public sealed class MovieOptions
    {
        private const int DEFAULT_PAGE_NUMBER = 1;
        private const int DEFAULT_PAGE_SIZE = 10;
        private static readonly IReadOnlyCollection<string> DefaultOrder = new List<string> { "title" };

        public static MovieOptions Empty(
            int pageNumber = DEFAULT_PAGE_NUMBER,
            int pageSize = DEFAULT_PAGE_SIZE)
        {
            return new() { PageNumber = pageNumber, PageSize = pageSize };
        }

        [FromQuery(Name = "page")]
        public int PageNumber { get; init; } = DEFAULT_PAGE_NUMBER;

        [FromQuery(Name = "limit")]
        public int PageSize { get; init; } = DEFAULT_PAGE_SIZE;

        [FromQuery(Name = "rated")]
        public string Rated { get; init; } = string.Empty;

        [FromQuery(Name = "runtime")]
        [RegularExpression(Expression.RuntimeExpression, ErrorMessage = ErrorMessage.RuntimeErrorMessage)]
        public string Runtime { get; init; } = string.Empty;

        [FromQuery(Name = "title")]
        public string Title { get; init; } = string.Empty;

        [FromQuery(Name = "year")]
        [RegularExpression(Expression.YearExpression, ErrorMessage = ErrorMessage.YearErrorMessage)]
        public string Year { get; init; } = string.Empty;

        [FromQuery(Name = "type")]
        [RegularExpression(Expression.TypeExpression, ErrorMessage = ErrorMessage.TypeErrorMessage)]
        public string Type { get; set; } = string.Empty;

        [FromQuery(Name = "order")]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        public IReadOnlyCollection<string>? Order { get; init; } = DefaultOrder;

        [FromQuery(Name = "cast")]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        public IReadOnlyCollection<string>? Cast { get; init; } = Enumerable.Empty<string>().ToList();

        [FromQuery(Name = "genres")]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        public IReadOnlyCollection<string>? Genres { get; init; } = Enumerable.Empty<string>().ToList();

        [FromQuery(Name = "directors")]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        public IReadOnlyCollection<string>? Directors { get; init; } = Enumerable.Empty<string>().ToList();

        private static class Expression
        {
            internal const string YearExpression = @"^(gt|gte|lt|lte|eq):[1-9]{1}\d{3}$";
            internal const string RuntimeExpression = @"^(gt|gte|lt|lte|eq):[1-9]{1}\d*$";
            internal const string TypeExpression = @"^(movie|series)$";
        }

        private static class ErrorMessage
        {
            internal const string YearErrorMessage = "Invalid 'year' specified."
                + " A valid expression must be in the form of 'gt:2018', gte:2018, lt:2018, lte:2018, eq:2018";

            internal const string RuntimeErrorMessage = "Invalid 'runtime' specified."
                + " A valid expression must be in the form of 'gt:125', gte:125, lt:125, lte:125, eq:125";

            internal const string TypeErrorMessage = "Invalid 'type' specified. The value for type must be either 'movie' or 'series'";
        }
    }
}
