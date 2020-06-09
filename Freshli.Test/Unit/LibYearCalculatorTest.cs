using System;
using Xunit;

namespace Freshli.Test.Unit
{
  public class LibYearCalculatorTest
  {
    [Fact]
    public void Tokenizer()
    {
      // | theseer/tokenizer  | 1.1.0           | 2017-04-07 | 1.1.3          | 2019-06-13 | 2.18
      var calculator = new LibYearCalculator(null, null);

      var oldVersion = new VersionInfo("1.1.0", new DateTime(2017, 04, 07));
      var newVersion = new VersionInfo("1.1.3", new DateTime(2019, 06, 13));

      Assert.Equal(2.18, calculator.Compute(oldVersion, newVersion), 2);
    }

    [Fact]
    public void Parser()
    {
      // | orchestra/parser   | v3.6.0          | 2018-02-08 | v4.0.0         | 2019-08-28 | 1.55
      var calculator = new LibYearCalculator(null, null);

      var oldVersion = new VersionInfo("v3.6.0", new DateTime(2018, 02, 08));
      var newVersion = new VersionInfo("v4.0.0", new DateTime(2019, 08, 28));

      Assert.Equal(1.55, calculator.Compute(oldVersion, newVersion), 2);
    }

    [Fact]
    public void VfsStream()
    {
      // | mikey179/vfsstream | v1.4.0          | 2014-09-14 | v1.6.8         | 2019-10-30 | 5.13
      var calculator = new LibYearCalculator(null, null);

      var oldVersion = new VersionInfo("v1.1.4", new DateTime(2014, 09, 14));
      var newVersion = new VersionInfo("v1.6.8", new DateTime(2019, 10, 30));

      Assert.Equal(5.13, calculator.Compute(oldVersion, newVersion), 2);
    }
  }
}
