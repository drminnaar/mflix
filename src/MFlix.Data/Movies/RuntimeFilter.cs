using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using MFlix.Data.Movies.Models;
using MongoDB.Driver;

namespace MFlix.Data.Movies
{
    public sealed class RuntimeFilter
    {
        private const string RuntimeFilterPattern = "^(gt|gte|lt|lte|eq|ne):([1-9]{1}[0-9]*)$";
        private readonly IReadOnlyDictionary<string, FilterDefinition<Movie>> _filterDefinitions;

        public RuntimeFilter(string runtimeFilter)
        {
            if (string.IsNullOrWhiteSpace(runtimeFilter))
                throw new ArgumentException($"'{nameof(runtimeFilter)}' cannot be null or whitespace.", nameof(runtimeFilter));

            if (!Regex.IsMatch(runtimeFilter, RuntimeFilterPattern))
                throw new ArgumentException("The runtime must be specified in the format of 'gt:10', 'gte:10', 'lt:10', 'lte:10', 'eq:10', 'ne:10'", nameof(runtimeFilter));

            var filterTokens = runtimeFilter.Split(":");
            EqualityOperator = filterTokens[0];
            var runtime = Runtime = int.Parse(filterTokens[1], CultureInfo.InvariantCulture);

            _filterDefinitions = new Dictionary<string, FilterDefinition<Movie>>
            {
                { "gt", Builders<Movie>.Filter.Gt(m => m.Runtime, runtime) },
                { "gte", Builders<Movie>.Filter.Gte(m => m.Runtime, runtime) },
                { "lt", Builders<Movie>.Filter.Lt(m => m.Runtime, runtime) },
                { "lte", Builders<Movie>.Filter.Lte(m => m.Runtime, runtime) },
                { "eq", Builders<Movie>.Filter.Eq(m => m.Runtime, runtime) },
                { "ne", Builders<Movie>.Filter.Ne(m => m.Runtime, runtime) }
            };
        }

        public string EqualityOperator { get; private set; } = string.Empty;
        public int Runtime { get; private set; }

        public FilterDefinition<Movie> Definition => _filterDefinitions[EqualityOperator];
    }
}
