using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Corgibytes.Freshli.Lib.Util;
using Moq;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit {
  public class AnalysisDatesTest {
    private Mock<IFileHistory> _history = new();

    [Fact]
    public void SingleDate() {
      ConfigureDateHistory();
      var analysisDates = new AnalysisDates(
        _history.Object,
        asOf: ParseExact("2020-01-01T00:00:00.0000000Z")
      );
      Assert.Collection(
        analysisDates,
        value => Assert.Equal(value, ParseExact("2020-01-01T00:00:00.0000000Z"))
      );
    }

    [Fact]
    public void DateRange() {
      ConfigureDateHistoryByYear(year: 2017, count: 3);
      var stopDate = ParseExact("2020-01-01T23:59:59.9999999Z");
      var analysisDates = new AnalysisDates(
        _history.Object,
        asOf: stopDate
      );

      var expectedDates = new List<DateTimeOffset>() {
        ParseExact("2017-01-01T23:59:59.9999999Z")
      };
      var currentDate = ParseExact("2017-02-01T00:00:00.0000000Z");
      while (currentDate <= stopDate) {
        expectedDates.Add(currentDate);
        currentDate = currentDate.AddMonths(1);
      }

      Assert.Equal(expectedDates, analysisDates);
    }

    [Fact]
    public void DateRangeUsesTimeZoneFromAsOfValue() {
      ConfigureDateHistoryByYear(year: 2017, count: 3);
      var stopDate = ParseExact("2020-01-01T23:59:59.9999999-08:00");
      var analysisDates = new AnalysisDates(
        _history.Object,
        asOf: stopDate
      );

      var expectedDates = new List<DateTimeOffset>() {
        ParseExact("2017-01-01T23:59:59.9999999Z")
      };
      var currentDate = ParseExact("2017-02-01T00:00:00.0000000-08:00");
      while (currentDate <= stopDate) {
        expectedDates.Add(currentDate);
        currentDate = currentDate.AddMonths(1);
      }

      Assert.Equal(expectedDates, analysisDates);
    }

    [Fact]
    public void DateRangeStartsExactlyAtStartOfMonth() {
      ConfigureDateHistory(
        "2017-01-01T00:00:00.0000000Z",
        "2018-01-01T23:59:59.9999999Z",
        "2019-01-01T23:59:59.9999999Z"
      );
      var stopDate = ParseExact("2020-01-01T23:59:59.9999999Z");
      var analysisDates = new AnalysisDates(
        _history.Object,
        asOf: stopDate
      );

      var expectedDates = new List<DateTimeOffset>() {
        ParseExact("2017-01-01T00:00:00.0000000Z")
      };
      var currentDate = ParseExact("2017-02-01T00:00:00.0000000Z");
      while (currentDate <= stopDate) {
        expectedDates.Add(currentDate);
        currentDate = currentDate.AddMonths(1);
      }

      Assert.Equal(expectedDates, analysisDates);
    }

    [Fact]
    public void DateRangeStopsOnSpeciedDate() {
      ConfigureDateHistoryByYear(year: 2017, count: 4);
      var stopDate = ParseExact("2019-01-01T23:59:59.9999999Z");
      var analysisDates = new AnalysisDates(
        _history.Object,
        asOf: stopDate
      );

      var expectedDates = new List<DateTimeOffset>() {
        ParseExact("2017-01-01T23:59:59.9999999Z")
      };
      var currentDate = ParseExact("2017-02-01T00:00:00.0000000Z");
      while (currentDate <= stopDate) {
        expectedDates.Add(currentDate);
        currentDate = currentDate.AddMonths(1);
      }

      Assert.Equal(expectedDates, analysisDates);
    }

    [Fact]
    public void FirstFileCreatedBeforeStartOfMonthSingleFileVersion() {
      ConfigureDateHistory("2016-12-15T00:00:00.0000000Z");
      var stopDate = ParseExact("2019-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        _history.Object,
        asOf: stopDate
      );

      var expectedDates = new List<DateTimeOffset>() {
        ParseExact("2016-12-15T00:00:00.0000000Z")
      };
      var currentDate = ParseExact("2017-01-01T00:00:00.0000000Z");
      while (currentDate <= stopDate) {
        expectedDates.Add(currentDate);
        currentDate = currentDate.AddMonths(1);
      }

      Assert.Equal(expectedDates, analysisDates);
    }

    [Fact]
    public void FirstFileCreatedAfterStartOfMonthMultipleFileVersions() {
      ConfigureDateHistory(
        "2016-12-15T00:00:00.0000000Z",
        "2017-12-15T00:00:00.0000000Z"
      );
      var stopDate = ParseExact("2019-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        _history.Object,
        asOf: stopDate
      );

      var expectedDates = new List<DateTimeOffset>() {
        ParseExact("2016-12-15T00:00:00.0000000Z")
      };
      var currentDate = ParseExact("2017-01-01T00:00:00.0000000Z");
      while (currentDate <= stopDate) {
        expectedDates.Add(currentDate);
        currentDate = currentDate.AddMonths(1);
      }

      Assert.Equal(expectedDates, analysisDates);
    }

    [Fact]
    public void FirstFileCreatedOnStartOfMonthSingleFileVersion() {
      ConfigureDateHistory("2016-12-01T02:00:00.0000000Z");
      var stopDate = ParseExact("2019-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        _history.Object,
        asOf: stopDate
      );

      var expectedDates = new List<DateTimeOffset>() {
        ParseExact("2016-12-01T02:00:00.0000000Z")
      };
      var currentDate = ParseExact("2017-01-01T00:00:00.0000000Z");
      while (currentDate <= stopDate) {
        expectedDates.Add(currentDate);
        currentDate = currentDate.AddMonths(1);
      }

      Assert.Equal(expectedDates, analysisDates);
    }

    [Fact]
    public void FirstFileCreatedOnStartOfMonthMultipleFileVersions() {
      ConfigureDateHistory(
        "2016-12-01T02:00:00.0000000Z",
        "2017-12-01T02:00:00.0000000Z"
      );
      var stopDate = ParseExact("2019-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        _history.Object,
        asOf: stopDate
      );

      var expectedDates = new List<DateTimeOffset>() {
        ParseExact("2016-12-01T02:00:00.0000000Z")
      };
      var currentDate = ParseExact("2017-01-01T00:00:00.0000000Z");
      while (currentDate <= stopDate) {
        expectedDates.Add(currentDate);
        currentDate = currentDate.AddMonths(1);
      }

      Assert.Equal(expectedDates, analysisDates);
    }


    [Fact]
    public void FileDateIsNewerThanAsOfDateAndHistoryOnlyContainsOneVersion() {
      ConfigureDateHistory("2020-01-20T00:00:00.0000000Z");
      var stopDate = ParseExact("2020-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        _history.Object,
        asOf: stopDate
      );

      Assert.Equal(
        new List<DateTimeOffset> {ParseExact("2020-01-01T00:00:00.0000000Z")},
        analysisDates
      );
    }

    private static DateTimeOffset ParseExact(string value) {
      return DateTimeOffset.ParseExact(value,
        "o",
        CultureInfo.InvariantCulture,
        DateTimeStyles.RoundtripKind
      );
    }

    private void ConfigureDateHistory(params string[] dateStrings)
    {
      _history.Setup(mock => mock.Dates).Returns(
        dateStrings.Select(ParseExact).ToList()
      );
    }

    private void ConfigureDateHistoryByYear(int year, int count) {
      var dateStrings = Enumerable.
        Range(year, count).
        Select(year => $"{year}-01-01T23:59:59.9999999Z").
        ToArray();
      ConfigureDateHistory(dateStrings);
    }
  }
}
