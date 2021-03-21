using System.Collections.Generic;
using System.Linq;
using MFlix.Data.Movies.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MFlix.Data.Movies
{
    public interface IMovieFilterBuilder
    {
        FilterDefinition<Movie> BuildAsLogicalAnd();
        FilterDefinition<Movie> BuildAsLogicalOr();
        MovieFilterBuilder ClearFilters();
        MovieFilterBuilder WhereCastInAny(IReadOnlyCollection<string> cast);
        MovieFilterBuilder WhereDirectorsInAny(IReadOnlyCollection<string> directors);
        MovieFilterBuilder WhereGenresInAny(IReadOnlyCollection<string> genres);
        MovieFilterBuilder WhereRatedEquals(string rated);
        MovieFilterBuilder WhereRuntime(string runtime);
        MovieFilterBuilder WhereTitleLike(string title);
        MovieFilterBuilder WhereTypeEquals(string type);
        MovieFilterBuilder WhereYear(string year);
    }

    public sealed class MovieFilterBuilder : IMovieFilterBuilder
    {
        private FilterDefinition<Movie> _castFilter = Builders<Movie>.Filter.Empty;
        private FilterDefinition<Movie> _directorsFilter = Builders<Movie>.Filter.Empty;
        private FilterDefinition<Movie> _genresFilter = Builders<Movie>.Filter.Empty;
        private FilterDefinition<Movie> _ratedFilter = Builders<Movie>.Filter.Empty;
        private FilterDefinition<Movie> _runtimeFilter = Builders<Movie>.Filter.Empty;
        private FilterDefinition<Movie> _titleFilter = Builders<Movie>.Filter.Empty;
        private FilterDefinition<Movie> _typeFilter = Builders<Movie>.Filter.Empty;
        private FilterDefinition<Movie> _yearFilter = Builders<Movie>.Filter.Empty;

        public FilterDefinition<Movie> BuildAsLogicalAnd() => Builders<Movie>.Filter.And(AllFilters());

        public FilterDefinition<Movie> BuildAsLogicalOr() => Builders<Movie>.Filter.Or(AllFilters());

        public MovieFilterBuilder ClearFilters()
        {
            _castFilter = Builders<Movie>.Filter.Empty;
            _directorsFilter = Builders<Movie>.Filter.Empty;
            _genresFilter = Builders<Movie>.Filter.Empty;
            _ratedFilter = Builders<Movie>.Filter.Empty;
            _runtimeFilter = Builders<Movie>.Filter.Empty;
            _titleFilter = Builders<Movie>.Filter.Empty;
            _typeFilter = Builders<Movie>.Filter.Empty;
            _yearFilter = Builders<Movie>.Filter.Empty;
            return this;
        }

        public MovieFilterBuilder WhereCastInAny(IReadOnlyCollection<string> cast)
        {
            if (cast is not null && cast.Any())
            {
                var regularExpressions = cast
                    .Select(member => new BsonRegularExpression(member, "i"))
                    .ToArray();

                _castFilter = Builders<Movie>
                    .Filter
                    .AnyIn("cast", regularExpressions);
            }
            return this;
        }

        public MovieFilterBuilder WhereDirectorsInAny(IReadOnlyCollection<string> directors)
        {
            if (directors is not null && directors.Any())
            {
                var regularExpressions = directors
                    .Select(director => new BsonRegularExpression(director, "i"))
                    .ToArray();

                _directorsFilter = Builders<Movie>
                    .Filter
                    .AnyIn("directors", regularExpressions);
            }
            return this;
        }

        public MovieFilterBuilder WhereGenresInAny(IReadOnlyCollection<string> genres)
        {
            if (genres is not null && genres.Any())
            {
                var regularExpressions = genres
                    .Select(genre => new BsonRegularExpression(genre, "i"))
                    .ToArray();

                _genresFilter = Builders<Movie>
                    .Filter
                    .AnyIn("genres", regularExpressions);
            }
            return this;
        }

        public MovieFilterBuilder WhereRatedEquals(string rated)
        {
            if (!string.IsNullOrWhiteSpace(rated))
            {
                _ratedFilter = Builders<Movie>
                    .Filter
                    .Regex(movie => movie.Rated, new BsonRegularExpression(rated, "i"));
            }
            return this;
        }

        public MovieFilterBuilder WhereRuntime(string runtime)
        {
            if (!string.IsNullOrWhiteSpace(runtime))
            {
                _runtimeFilter = new RuntimeFilter(runtime).Definition;
            }
            return this;
        }

        public MovieFilterBuilder WhereTitleLike(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                _titleFilter = Builders<Movie>
                    .Filter
                    .Regex(m => m.Title, new BsonRegularExpression(title, "i"));
            }
            return this;
        }

        public MovieFilterBuilder WhereTypeEquals(string type)
        {
            if (!string.IsNullOrWhiteSpace(type))
            {
                _typeFilter = Builders<Movie>
                    .Filter
                    .Regex(m => m.Type, new BsonRegularExpression(type, "i"));
            }
            return this;
        }

        public MovieFilterBuilder WhereYear(string year)
        {
            if (!string.IsNullOrWhiteSpace(year))
            {
                _yearFilter = new YearFilter(year).Definition;
            }

            return this;
        }

        private IReadOnlyCollection<FilterDefinition<Movie>> AllFilters() =>
            new List<FilterDefinition<Movie>>
            {
                _castFilter,
                _directorsFilter,
                _genresFilter,
                _ratedFilter,
                _runtimeFilter,
                _titleFilter,
                _typeFilter,
                _yearFilter
            };
    }
}
