using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace LibMetrics.Test.Unit
{
  public class GitFileHistoryTest
  {
    [Fact]
    public void Dates()
    {
      var assemblyPath = System.Reflection.Assembly.
        GetExecutingAssembly().Location;
      var rubyFixturePath = Path.Combine(
        Directory.GetParent(assemblyPath).ToString(),
        "fixtures",
        "ruby",
        "nokotest");

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

      var assemblyPath = System.Reflection.Assembly.
        GetExecutingAssembly().Location;
      var rubyFixturePath = Path.Combine(
        Directory.GetParent(assemblyPath).ToString(),
        "fixtures",
        "ruby",
        "nokotest");

      var history = new GitFileHistory(rubyFixturePath, "Gemfile.lock");
      var contents = history.ContentsAsOf(new DateTime(2017, 02, 01));

      Assert.Equal(expectedContents, contents);
    }
  }
}
