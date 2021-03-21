using System;
using System.Collections.Generic;
using System.Linq;
using MFlix.Data.Movies.Models;
using MongoDB.Driver;

namespace MFlix.Data.Movies
{
    public interface IMovieSortDefinition
    {
        SortDefinition<Movie> SortBy(IReadOnlyCollection<string> fields);
    }

    public sealed class MovieSortDefinition : IMovieSortDefinition
    {
        private readonly IReadOnlyDictionary<string, SortDefinition<Movie>> _sortDefinitions = CreateSortDefinitions();
        private readonly SortDefinition<Movie> _defaultSortDefinition;

        public MovieSortDefinition()
        {
            _defaultSortDefinition = _sortDefinitions["default"];
        }

        public SortDefinition<Movie> SortBy(IReadOnlyCollection<string> fields)
        {
            if (fields is null || !fields.Any())
                return _defaultSortDefinition;

            var sortDefinitions = new HashSet<string>(fields)
                .Select(field =>
                {
                    if (!_sortDefinitions.TryGetValue(field, out var sortDefinition))
                        throw SortDefinitionNotSupportedException(field);

                    return sortDefinition;
                })
                .ToList();

            return Builders<Movie>.Sort.Combine(sortDefinitions);
        }

        private static IReadOnlyDictionary<string, SortDefinition<Movie>> CreateSortDefinitions()
        {
            return new Dictionary<string, SortDefinition<Movie>>
            {
                { "default", Builders<Movie>.Sort.Ascending(m => m.Title) },
                { "title", Builders<Movie>.Sort.Ascending(m => m.Title) },
                { "-title", Builders<Movie>.Sort.Descending(m => m.Title) },
                { "runtime", Builders<Movie>.Sort.Ascending(m => m.Runtime) },
                { "-runtime", Builders<Movie>.Sort.Descending(m => m.Runtime) },
                { "year", Builders<Movie>.Sort.Ascending(m => m.Year) },
                { "-year", Builders<Movie>.Sort.Descending(m => m.Year) }
            };
        }

        private NotSupportedException SortDefinitionNotSupportedException(string field)
        {
            var exception = new NotSupportedException($"A sort definition has not been defined for the field '{field}'.");

            exception.Data.Add(
                key: "SupportedSortDefinitions",
                value: string.Join(", ", _sortDefinitions.Select(kvp => $"{{ {kvp.Key}:{kvp.Value} }}")));

            return exception;
        }
    }
}
