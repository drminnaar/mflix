using System;
using System.Collections.Generic;
using MFlix.Data.Movies;
using Xunit;

namespace MFlix.DataTests.Movies
{
    public sealed class RuntimeFilterTest
    {
        [Theory]
        [MemberData(nameof(ValidData))]
        public void Should_Get_Definition_For_Valid_RuntimeFilter(string runtimeFilter, string expectedOperator, int expectedRuntime)
        {
            // arrange
            var filter = new RuntimeFilter(runtimeFilter);

            // act
            var actualDefinition = filter.Definition;

            // assert
            Assert.Equal(expectedOperator, filter.EqualityOperator);
            Assert.Equal(expectedRuntime, filter.Runtime);
            Assert.NotNull(actualDefinition);
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public void Should_Error_For_Invalid_RuntimeFilter(string runtimeFilter)
        {
            // assert
            Assert.Throws<ArgumentException>(() => new RuntimeFilter(runtimeFilter));
        }

        public static IEnumerable<object[]> ValidData => new List<object[]>
            {
                new object[] { "gt:120", "gt", 120 },
                new object[] { "gte:635", "gte", 635 },
                new object[] { "lt:439", "lt", 439 },
                new object[] { "lte:45", "lte", 45},
                new object[] { "eq:95", "eq", 95 },
                new object[] { "ne:136", "ne", 136 }
            };

        public static IEnumerable<object[]> InvalidData => new List<object[]>
            {
                new object[] { "" },
                new object[] { "    " },
                new object[] { "gt" },
                new object[] { "gt:" },
                new object[] { "aa" },
                new object[] { "aa:1234" },
                new object[] { "aa:1234" },
                new object[] { "lt:aa" },
                new object[] { "lt:0123" },
                new object[] { "lt:00" },
            };
    }
}
