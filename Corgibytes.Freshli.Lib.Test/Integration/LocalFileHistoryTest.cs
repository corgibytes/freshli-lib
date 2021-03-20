using System;
using System.Collections.Generic;
using Corgibytes.Freshli.Lib.Util;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration {
  public class LocalFileHistoryTest {
    [Fact]
    public void Dates() {
      var emptyFixturePath = Fixtures.Path("empty");
      var history = new LocalFileHistory(emptyFixturePath, "readme.md");

      Assert.InRange(
        history.Dates[0],
        DateTimeOffset.UtcNow.ToStartOfDay(),
        DateTimeOffset.UtcNow.ToEndOfDay()
      );
      Assert.Equal(expected: 1, history.Dates.Count);
    }

    [Fact]
    public void ContentsAsOf() {
      var emptyFixturePath = Fixtures.Path("empty");
      var history = new LocalFileHistory(emptyFixturePath, "readme.md");

      var expectedContents = "This directory is meant to simulate a project " +
        "which does not have any manifest" +
        Environment.NewLine + "files in " + "it." + Environment.NewLine;
      Assert.Equal(
        expectedContents,
        history.ContentsAsOf(DateTimeOffset.Now.ToStartOfDay())
      );
    }
  }
}
