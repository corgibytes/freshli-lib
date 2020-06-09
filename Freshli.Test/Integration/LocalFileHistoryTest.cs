using System;
using System.Collections.Generic;
using Xunit;

namespace Freshli.Test.Integration
{
  public class LocalFileHistoryTest
  {
    [Fact]
    public void Dates()
    {
      var emptyFixturePath = Fixtures.Path("empty");
      var history = new LocalFileHistory(emptyFixturePath, "readme.md");

      Assert.Equal(new List<DateTime> { DateTime.Today }, history.Dates);
    }

    [Fact]
    public void ContentsAsOf()
    {
      var emptyFixturePath = Fixtures.Path("empty");
      var history = new LocalFileHistory(emptyFixturePath, "readme.md");

      var expectedContents = "This directory is meant to simulate a project " + 
                             "which does not have any manifest" +
                             Environment.NewLine + "files in " + "it." + Environment.NewLine;
      Assert.Equal(expectedContents, history.ContentsAsOf(DateTime.Today));
    }
  }
}
