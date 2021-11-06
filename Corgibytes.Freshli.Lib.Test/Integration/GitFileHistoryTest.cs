using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    public class GitFileHistoryTest
    {
        [Fact]
        public void Dates()
        {
            var rubyFixturePath = Fixtures.Path("ruby", "nokotest");
            var history = new GitFileHistory(rubyFixturePath, "Gemfile.lock");

            var expectedDates = new List<DateTimeOffset>() {
        new(2017, 01, 01, 00, 01, 29, TimeSpan.FromHours(-8)),
        new(2018, 01, 01, 00, 00, 59, TimeSpan.FromHours(-8)),
        new(2019, 01, 01, 00, 00, 46, TimeSpan.FromHours(-8))
      };

            Assert.Equal(expectedDates, history.Dates);
        }

        [Fact]
        public void ContentsAsOf2017()
        {
            var expectedContents =
              "GEM\n" +
              "  remote: https://rubygems.org/\n" +
              "  specs:\n" +
              "    mini_portile2 (2.1.0)\n" +
              "    nokogiri (1.7.0)\n" +
              "      mini_portile2 (~> 2.1.0)\n" +
              "\n" +
              "PLATFORMS\n" +
              "  ruby\n" +
              "\n" +
              "DEPENDENCIES\n" +
              "  nokogiri (= 1.7.0)\n" +
              "\n" +
              "BUNDLED WITH\n" +
              "   2.1.3\n";

            var rubyFixturePath = Fixtures.Path("ruby", "nokotest");
            var history = new GitFileHistory(rubyFixturePath, "Gemfile.lock");
            var contents = history.ContentsAsOf(
              new DateTimeOffset(2017, 02, 01, 00, 00, 00, TimeSpan.Zero)
            );

            Assert.Equal(expectedContents, contents);
        }
    }
}
