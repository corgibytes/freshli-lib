using System;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit {
  public class LibYearCalculatorTest {
    [Fact]
    public void Tokenizer() {
      var oldVersion = BuildVersion("1.1.0", 2017, 04, 07);
      var newVersion = BuildVersion("1.1.3", 2019, 06, 13);

      Assert.Equal(2.18, LibYearCalculator.Compute(oldVersion, newVersion), 2);
    }

    [Fact]
    public void Parser() {
      var oldVersion = BuildVersion("v3.6.0", 2018, 02, 08);
      var newVersion = BuildVersion("v4.0.0", 2019, 08, 28);

      Assert.Equal(1.55, LibYearCalculator.Compute(oldVersion, newVersion), 2);
    }

    [Fact]
    public void VfsStream() {
      var oldVersion = BuildVersion("v1.1.4", 2014, 09, 14);
      var newVersion = BuildVersion("v1.6.8", 2019, 10, 30);

      Assert.Equal(5.13, LibYearCalculator.Compute(oldVersion, newVersion), 2);
    }

    [Fact]
    public void Compute() {
      var olderVersion = BuildVersion("1.7.0", 2016, 12, 27);
      var newerVersion = BuildVersion("1.7.1", 2017, 03, 20);

      Assert.Equal(
        0.227,
        LibYearCalculator.Compute(olderVersion, newerVersion),
        3
      );
    }

    private static SemVerVersionInfo BuildVersion(
      string rawVersion,
      int year,
      int month,
      int day
    ) {
      return new(
        rawVersion,
        new DateTimeOffset(year, month, day, 00, 00, 00, TimeSpan.Zero)
      );
    }
  }
}
