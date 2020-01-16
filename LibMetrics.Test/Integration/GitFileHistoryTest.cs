using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace LibMetrics.Test.Integration
{
  public class GitFileHistoryTest
  {
    [Fact]
    public void Dates()
    {
      var rubyFixturePath = Fixtures.Path("ruby", "nokotest");
      var history = new GitFileHistory(rubyFixturePath, "Gemfile.lock");

      var expectedDates = new List<DateTime>()
      {
        new DateTime(2017, 01, 01),
        new DateTime(2018, 01, 01),
        new DateTime(2019, 01, 01)
      };

      Assert.Equal(expectedDates, history.Dates);
    }

    [Fact]
    public void ContentsAsOf2017()
    {
      var expectedContents = @"GEM
  remote: https://rubygems.org/
  specs:
    mini_portile2 (2.1.0)
    nokogiri (1.7.0)
      mini_portile2 (~> 2.1.0)

PLATFORMS
  ruby

DEPENDENCIES
  nokogiri (= 1.7.0)

BUNDLED WITH
   2.1.3
";

      var rubyFixturePath = Fixtures.Path("ruby", "nokotest");
      var history = new GitFileHistory(rubyFixturePath, "Gemfile.lock");
      var contents = history.ContentsAsOf(new DateTime(2017, 02, 01));

      Assert.Equal(expectedContents, contents);
    }
  }
}
