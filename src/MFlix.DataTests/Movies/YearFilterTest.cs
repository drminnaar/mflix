using System;
using System.Collections.Generic;
using MFlix.Data.Movies;
using Xunit;

namespace MFlix.DataTests.Movies
{
    public sealed class YearFilterTest
    {
        [Theory]
        [MemberData(nameof(ValidData))]
        public void Should_Get_Definition_For_Valid_YearFilter(string yearFilter, string expectedOperator, int expectedYear)
        {
            // arrange
            var filter = new YearFilter(yearFilter);

            // act
            var actualDefinition = filter.Definition;

            // assert
            Assert.Equal(expectedOperator, filter.EqualityOperator);
            Assert.Equal(expectedYear, filter.Year);
            Assert.NotNull(actualDefinition);
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public void Should_Error_For_Invalid_YearFilter(string yearFilter)
        {
            // assert
            Assert.Throws<ArgumentException>(() => new YearFilter(yearFilter));
        }

        public static IEnumerable<object[]> ValidData => new List<object[]>
            {
                new object[] { "gt:1980", "gt", 1980 },
                new object[] { "gte:1980", "gte", 1980 },
                new object[] { "lt:1980", "lt", 1980 },
                new object[] { "lte:1980", "lte", 1980 },
                new object[] { "eq:1980", "eq", 1980 },
                new object[] { "ne:1980", "ne", 1980 }
            };

        public static IEnumerable<object[]> InvalidData => new List<object[]>
            {
                new object[] { "" },
                new object[] { "gt" },
                new object[] { "gt:" },
                new object[] { "aa" },
                new object[] { "aa:1234" },
                new object[] { "aa:1234" },
                new object[] { "lt:1" },
                new object[] { "lt:12" },
                new object[] { "lt:123" },
                new object[] { "lt:aa" },
                new object[] { "lt:0123" }
            };
    }
}
