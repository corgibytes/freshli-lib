using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Corgibytes.Freshli.Lib.Util;
using Moq;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit {
  public class AnalysisDatesTest {
    [Fact]
    public void SingleDate() {
      var history = new Mock<IFileHistory>();
      var dates = new List<DateTimeOffset>();
      history.Setup(mock => mock.Dates).Returns(dates);
      var analysisDates = new AnalysisDates(
        history.Object,
        asOf: ParseExact("2020-01-01T00:00:00.0000000Z")
      );
      Assert.Collection(
        analysisDates,
        value => Assert.Equal(value, ParseExact("2020-01-01T00:00:00.0000000Z"))
      );
    }

    [Fact]
    public void DateRange() {
      var history = new Mock<IFileHistory>();
      var dates = new List<DateTimeOffset>() {
        ParseExact("2017-01-01T23:59:59.9999999Z"),
        ParseExact("2018-01-01T23:59:59.9999999Z"),
        ParseExact("2019-01-01T23:59:59.9999999Z")
      };

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = ParseExact("2020-01-01T23:59:59.9999999Z");
      var analysisDates = new AnalysisDates(
        history.Object,
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
      var history = new Mock<IFileHistory>();
      var dates = new List<DateTimeOffset>() {
        ParseExact("2017-01-01T23:59:59.9999999Z"),
        ParseExact("2018-01-01T23:59:59.9999999Z"),
        ParseExact("2019-01-01T23:59:59.9999999Z")
      };

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = ParseExact("2020-01-01T23:59:59.9999999-08:00");
      var analysisDates = new AnalysisDates(
        history.Object,
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
      var history = new Mock<IFileHistory>();
      var dates = new List<DateTimeOffset>() {
        ParseExact("2017-01-01T00:00:00.0000000Z"),
        ParseExact("2018-01-01T23:59:59.9999999Z"),
        ParseExact("2019-01-01T23:59:59.9999999Z")
      };

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = ParseExact("2020-01-01T23:59:59.9999999Z");
      var analysisDates = new AnalysisDates(
        history.Object,
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
      var history = new Mock<IFileHistory>();
      var dates = new List<DateTimeOffset>() {
        ParseExact("2017-01-01T23:59:59.9999999Z"),
        ParseExact("2018-01-01T23:59:59.9999999Z"),
        ParseExact("2019-01-01T23:59:59.9999999Z"),
        ParseExact("2020-01-01T23:59:59.9999999Z")
      };

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = ParseExact("2019-01-01T23:59:59.9999999Z");
      var analysisDates = new AnalysisDates(
        history.Object,
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
      var history = new Mock<IFileHistory>();
      var dates =
        new List<DateTimeOffset>() {ParseExact("2016-12-15T00:00:00.0000000Z")};

      // TODO Is the name stopDate misleading as it is now inclusive?
      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = ParseExact("2019-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        history.Object,
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
      var history = new Mock<IFileHistory>();
      var dates =
        new List<DateTimeOffset>() {
          ParseExact("2016-12-15T00:00:00.0000000Z"),
          ParseExact("2017-12-15T00:00:00.0000000Z")
        };

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = ParseExact("2019-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        history.Object,
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
      var history = new Mock<IFileHistory>();
      var dates =
        new List<DateTimeOffset>() {ParseExact("2016-12-01T02:00:00.0000000Z")};

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = ParseExact("2019-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        history.Object,
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
      var history = new Mock<IFileHistory>();
      var dates =
        new List<DateTimeOffset>() {
          ParseExact("2016-12-01T02:00:00.0000000Z"),
          ParseExact("2017-12-01T02:00:00.0000000Z")
        };

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = ParseExact("2019-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        history.Object,
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
      var history = new Mock<IFileHistory>();
      var dates =
        new List<DateTimeOffset>() {ParseExact("2020-01-20T00:00:00.0000000Z")};

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = ParseExact("2020-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        history.Object,
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
  }
}
