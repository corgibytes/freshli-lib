using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Moq;
using Xunit;

namespace Freshli.Test.Unit
{
  public class AnalysisDatesTest
  {
    private static DateTime ParseExact(string value)
    {
      return DateTime.ParseExact(value, "o", CultureInfo.InvariantCulture,
        DateTimeStyles.RoundtripKind);
    }

    [Fact]
    public void SingleDate()
    {
      var history = new Mock<IFileHistory>();
      var dates = new List<DateTime>();
      history.Setup(mock => mock.Dates).Returns(dates);
      var analysisDates = new AnalysisDates(
        history.Object,
        asOf: ParseExact("2020-01-01T00:00:00.0000000Z"));
      Assert.Collection(
        analysisDates,
        value => Assert.Equal(value, ParseExact("2020-01-01T00:00:00.0000000Z")));
    }

    [Fact]
    public void DateRange()
    {
      var history = new Mock<IFileHistory>();
      var dates = new List<DateTime>()
      {
        ParseExact("2017-01-01T00:00:00.0000000Z"),
        ParseExact("2018-01-01T00:00:00.0000000Z"),
        ParseExact("2019-01-01T00:00:00.0000000Z")
      };

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = ParseExact("2020-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        history.Object,
        asOf: stopDate);

      var expectedDates = new List<DateTime>();
      var currentDate = dates.First();
      while (currentDate <= stopDate)
      {
        expectedDates.Add(currentDate);
        currentDate = currentDate.AddMonths(1);
      }

      Assert.Equal(expectedDates, analysisDates);
    }

    [Fact]
    public void DateRangeStopsOnSpeciedDate()
    {
      var history = new Mock<IFileHistory>();
      var dates = new List<DateTime>()
      {
        ParseExact("2017-01-01T00:00:00.0000000Z"),
        ParseExact("2018-01-01T00:00:00.0000000Z"),
        ParseExact("2019-01-01T00:00:00.0000000Z"),
        ParseExact("2020-01-01T00:00:00.0000000Z")
      };

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = ParseExact("2019-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        history.Object,
        asOf: stopDate);

      var expectedDates = new List<DateTime>();
      var currentDate = dates.First();
      while (currentDate <= stopDate)
      {
        expectedDates.Add(currentDate);
        currentDate = currentDate.AddMonths(1);
      }

      Assert.Equal(expectedDates, analysisDates);
    }

    [Fact]
    public void FirstFileCreatedBeforeStartOfMonth()
    {
      var history = new Mock<IFileHistory>();
      var dates = new List<DateTime>()
      {
        ParseExact("2016-12-15T00:00:00.0000000Z")
      };

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = ParseExact("2019-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        history.Object,
        asOf: stopDate);

      Assert.Equal(ParseExact("2017-01-01T00:00:00.0000000Z"), analysisDates.First());
    }

    [Fact]
    public void FileDateIsNewerThanAsOfDateAndHistoryOnlyContainsOneVersion()
    {
      var history = new Mock<IFileHistory>();
      var dates = new List<DateTime>()
      {
        ParseExact("2020-01-20T00:00:00.0000000Z")
      };

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = ParseExact("2020-01-01T00:00:00.0000000Z");
      var analysisDates = new AnalysisDates(
        history.Object,
        asOf: stopDate);

      Assert.Equal(new List<DateTime> {ParseExact("2020-01-01T00:00:00.0000000Z")}, analysisDates);
    }
  }
}
