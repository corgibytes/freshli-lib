using System;
using Xunit;

namespace Freshli.Test.Unit {
  public class LibYearCalculatorTest {
    [Fact]
    public void Tokenizer() {
      var calculator = new LibYearCalculator(null, null);

      var oldVersion = new VersionInfo("1.1.0", new DateTime(2017, 04, 07));
      var newVersion = new VersionInfo("1.1.3", new DateTime(2019, 06, 13));

      Assert.Equal(2.18, calculator.Compute(oldVersion, newVersion), 2);
    }

    [Fact]
    public void Parser() {
      var calculator = new LibYearCalculator(null, null);

      var oldVersion = new VersionInfo("v3.6.0", new DateTime(2018, 02, 08));
      var newVersion = new VersionInfo("v4.0.0", new DateTime(2019, 08, 28));

      Assert.Equal(1.55, calculator.Compute(oldVersion, newVersion), 2);
    }

    [Fact]
    public void VfsStream() {
      var calculator = new LibYearCalculator(null, null);

      var oldVersion = new VersionInfo("v1.1.4", new DateTime(2014, 09, 14));
      var newVersion = new VersionInfo("v1.6.8", new DateTime(2019, 10, 30));

      Assert.Equal(5.13, calculator.Compute(oldVersion, newVersion), 2);
    }
  }
}
