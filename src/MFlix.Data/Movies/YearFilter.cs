using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using MFlix.Data.Movies.Models;
using MongoDB.Driver;

namespace MFlix.Data.Movies
{
    public sealed class YearFilter
    {
        private const string YearFilterPattern = "^(gt|gte|lt|lte|eq|ne):([1-9]{1}[0-9]{1}[0-9]{1}[0-9]{1})$";
        private readonly IReadOnlyDictionary<string, FilterDefinition<Movie>> _filterDefinitions;

        public YearFilter(string yearFilter)
        {
            if (string.IsNullOrWhiteSpace(yearFilter))
                throw new ArgumentException($"'{nameof(yearFilter)}' cannot be null or whitespace.", nameof(yearFilter));

            if (!Regex.IsMatch(yearFilter, YearFilterPattern))
                throw new ArgumentException("The year must be specified in the format of 'gt:1980', 'gte:1980', 'lt:1980', 'lte:1980', 'eq:1980', 'ne:1980'", nameof(yearFilter));

            var filterTokens = yearFilter.Split(":");
            EqualityOperator = filterTokens[0];
            var year = Year = int.Parse(filterTokens[1], CultureInfo.InvariantCulture);

            _filterDefinitions = new Dictionary<string, FilterDefinition<Movie>>
            {
                { "gt", Builders<Movie>.Filter.Gt(movie => movie.Year, year) },
                { "gte", Builders<Movie>.Filter.Gte(movie => movie.Year, year) },
                { "lt", Builders<Movie>.Filter.Lt(movie => movie.Year, year) },
                { "lte", Builders<Movie>.Filter.Lte(movie => movie.Year, year) },
                { "eq", Builders<Movie>.Filter.Eq(movie => movie.Year, year) },
                { "ne", Builders<Movie>.Filter.Ne(movie => movie.Year, year) }
            };
        }

        public string EqualityOperator { get; private set; } = string.Empty;
        public int Year { get; private set; }

        public FilterDefinition<Movie> Definition => _filterDefinitions[EqualityOperator];
    }
}
