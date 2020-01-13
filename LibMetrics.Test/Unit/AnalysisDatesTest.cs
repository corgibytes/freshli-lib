using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

namespace LibMetrics.Test.Unit
{
  public class AnalysisDatesTest
  {
    [Fact]
    public void SingleDate()
    {
      var history = new Mock<IFileHistory>();
      var dates = new List<DateTime>();
      history.Setup(mock => mock.Dates).Returns(dates);
      var analysisDates = new AnalysisDates(
        history.Object,
        asOf: new DateTime(2020, 01, 01));
      Assert.Collection(
        analysisDates,
        value => Assert.Equal(value, new DateTime(2020, 01, 01)));
    }

    [Fact]
    public void DateRange()
    {
      var history = new Mock<IFileHistory>();
      var dates = new List<DateTime>()
      {
        new DateTime(2017, 01, 01),
        new DateTime(2018, 01, 01),
        new DateTime(2019, 01, 01)
      };

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = new DateTime(2020, 01, 01);
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
        new DateTime(2017, 01, 01),
        new DateTime(2018, 01, 01),
        new DateTime(2019, 01, 01),
        new DateTime(2020, 01, 01)
      };

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = new DateTime(2019, 01, 01);
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
        new DateTime(2016, 12, 15)
      };

      history.Setup(mock => mock.Dates).Returns(dates);
      var stopDate = new DateTime(2019, 01, 01);
      var analysisDates = new AnalysisDates(
        history.Object,
        asOf: stopDate);

      Assert.Equal(new DateTime(2017, 01, 01), analysisDates.First());
    }
  }
}
