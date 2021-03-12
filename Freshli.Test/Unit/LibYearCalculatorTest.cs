using System;
using Xunit;

namespace Freshli.Test.Unit {
  public class LibYearCalculatorTest {
    [Fact]
    public void Tokenizer() {
      var oldVersion = new SemVerVersionInfo("1.1.0",
        new DateTimeOffset(2017, 04, 07, 00, 00, 00, TimeSpan.Zero));
      var newVersion = new SemVerVersionInfo("1.1.3",
        new DateTimeOffset(2019, 06, 13, 00, 00, 00, TimeSpan.Zero));

      Assert.Equal(2.18, LibYearCalculator.Compute(oldVersion, newVersion), 2);
    }

    [Fact]
    public void Parser() {
      var oldVersion = new SemVerVersionInfo("v3.6.0",
        new DateTimeOffset(2018, 02, 08, 00, 00, 00, TimeSpan.Zero));
      var newVersion = new SemVerVersionInfo("v4.0.0",
        new DateTimeOffset(2019, 08, 28, 00, 00, 00, TimeSpan.Zero));

      Assert.Equal(1.55, LibYearCalculator.Compute(oldVersion, newVersion), 2);
    }

    [Fact]
    public void VfsStream() {
      var oldVersion = new SemVerVersionInfo("v1.1.4",
        new DateTimeOffset(2014, 09, 14, 00, 00, 00, 00, TimeSpan.Zero));
      var newVersion = new SemVerVersionInfo("v1.6.8",
        new DateTimeOffset(2019, 10, 30, 00, 00, 00, TimeSpan.Zero));

      Assert.Equal(5.13, LibYearCalculator.Compute(oldVersion, newVersion), 2);
    }
  }
}
