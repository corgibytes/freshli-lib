using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Corgibytes.Freshli.Lib.Util;
using Moq;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit
{
    public class AnalysisDatesTest
    {
        private Mock<IManifestHistory> _history = new();

        [Fact]
        public void SingleDate()
        {
            ConfigureDateHistory();
            var analysisDates = new AnalysisDates(_history.Object, asOf: ParseExact("2020-01-01T00:00:00.0000000Z"));
            Assert.Collection(analysisDates, value => Assert.Equal(value, ParseExact("2020-01-01T00:00:00.0000000Z")));
        }

        [Theory]
        [InlineData(new[] { 2017, 3 }, "2020-01-01T23:59:59.9999999Z", "2017-01-01T23:59:59.9999999Z", "2017-02-01T00:00:00.0000000Z")]
        [InlineData(new[] { 2017, 3 }, "2020-01-01T23:59:59.9999999-08:00", "2017-01-01T23:59:59.9999999Z", "2017-02-01T00:00:00.0000000-08:00")]
        [InlineData(new[] { 2017, 4 }, "2019-01-01T23:59:59.9999999Z", "2017-01-01T23:59:59.9999999Z", "2017-02-01T00:00:00.0000000Z")]
        public void RangeByYear(int[] historyConfig, string stopDateString, string firstDateString, string secondDateString)
        {
            ConfigureDateHistoryByYear(historyConfig);
            AssertAnalysisDates(firstDateString, secondDateString, stopDateString);
        }

        [Theory]
        [InlineData(
            new[]
            {
                "2017-01-01T00:00:00.0000000Z",
                "2018-01-01T23:59:59.9999999Z",
                "2019-01-01T23:59:59.9999999Z"
            },
            "2020-01-01T23:59:59.9999999Z",
            "2017-01-01T00:00:00.0000000Z",
            "2017-02-01T00:00:00.0000000Z"
        )]
        [InlineData(
            new[] { "2016-12-15T00:00:00.0000000Z" },
            "2019-01-01T00:00:00.0000000Z",
            "2016-12-15T00:00:00.0000000Z",
            "2017-01-01T00:00:00.0000000Z"
        )]
        [InlineData(
            new[]
            {
                "2016-12-15T00:00:00.0000000Z",
                "2017-12-15T00:00:00.0000000Z"
            },
            "2019-01-01T00:00:00.0000000Z",
            "2016-12-15T00:00:00.0000000Z",
            "2017-01-01T00:00:00.0000000Z"
        )]
        [InlineData(
            new[] { "2016-12-01T02:00:00.0000000Z" },
            "2019-01-01T00:00:00.0000000Z",
            "2016-12-01T02:00:00.0000000Z",
            "2017-01-01T00:00:00.0000000Z"
        )]
        [InlineData(
            new[] {
                "2016-12-01T02:00:00.0000000Z",
                "2017-12-01T02:00:00.0000000Z"
            },
            "2019-01-01T00:00:00.0000000Z",
            "2016-12-01T02:00:00.0000000Z",
            "2017-01-01T00:00:00.0000000Z"
        )]
        public void Range(string[] historyValues, string stopDateString, string firstDateString, string secondDateString)
        {
            ConfigureDateHistory(historyValues);
            AssertAnalysisDates(firstDateString, secondDateString, stopDateString);
        }

        [Fact]
        public void FileDateIsNewerThanAsOfDateAndHistoryOnlyContainsOneVersion()
        {
            ConfigureDateHistory("2020-01-20T00:00:00.0000000Z");
            var stopDate = ParseExact("2020-01-01T00:00:00.0000000Z");
            var analysisDates = new AnalysisDates(_history.Object, asOf: stopDate);

            Assert.Equal(new List<DateTimeOffset> { ParseExact("2020-01-01T00:00:00.0000000Z") }, analysisDates);
        }

        private DateTimeOffset ParseExact(string value)
        {
            return DateTimeOffset.ParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }

        private void ConfigureDateHistory(params string[] dateStrings)
        {
            _history.Setup(mock => mock.Dates).Returns(dateStrings.Select(ParseExact).ToList());
        }

        private void ConfigureDateHistoryByYear(int[] historyConfig)
        {
            ConfigureDateHistoryByYear(year: historyConfig[0], count: historyConfig[1]);
        }

        private void ConfigureDateHistoryByYear(int year, int count)
        {
            var dateStrings = Enumerable.
                Range(year, count).
                Select(year => $"{year}-01-01T23:59:59.9999999Z").
                ToArray();
            ConfigureDateHistory(dateStrings);
        }

        private List<DateTimeOffset> BuildExpectedDates(string firstDateString, string secondDateString, DateTimeOffset stopDate)
        {
            var expectedDates = new List<DateTimeOffset>() { ParseExact(firstDateString) };
            var currentDate = ParseExact(secondDateString);
            while (currentDate <= stopDate)
            {
                expectedDates.Add(currentDate);
                currentDate = currentDate.AddMonths(1);
            }

            return expectedDates;
        }

        private void AssertAnalysisDates(string firstDateString, string secondDateString, string stopDateString)
        {
            var stopDate = ParseExact(stopDateString);
            var analysisDates = new AnalysisDates(_history.Object, asOf: stopDate);

            var expectedDates = BuildExpectedDates(firstDateString, secondDateString, stopDate);

            Assert.Equal(expectedDates, analysisDates);
        }
    }
}
