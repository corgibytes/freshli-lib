using System;
using Xunit;

namespace LibMetrics.Test.Integration
{
  public class LibYearCalculatorTest
  {
    [Fact]
    public void ComputeAsOf()
    {
      var manifest = new BundlerManifest();
      manifest.Add("mini_portile2", "2.1.0");
      manifest.Add("nokogiri", "1.7.0");

      var repository = new RubyGemsRepository();

      var calculator = new LibYearCalculator(repository, manifest);

      Assert.Equal(
        0.227,
        calculator.ComputeAsOf(new DateTime(2017, 04, 01)),
        3);
    }

    [Fact]
    public void ComputeAsOfSmallValue()
    {
      var manifest = new BundlerManifest();
      manifest.Add("mini_portile2", "2.1.0");
      manifest.Add("nokogiri", "1.7.0");

      var repository = new RubyGemsRepository();

      var calculator = new LibYearCalculator(repository, manifest);

      Assert.Equal(
        0.022,
        calculator.ComputeAsOf(new DateTime(2017, 02, 01)),
        3);
    }

    [Fact]
    public void ComputeAsOfEdgeCase()
    {
      var manifest = new BundlerManifest();
      manifest.Add("mini_portile2", "2.3.0");
      manifest.Add("nokogiri", "1.8.1");

      var respository = new RubyGemsRepository();

      var calculator = new LibYearCalculator(respository, manifest);

      Assert.Equal(
        0,
        calculator.ComputeAsOf(new DateTime(2018, 01, 01)),
        3);
    }

    [Fact]
    public void ComputeAsOfOtherEdgeCase()
    {
      var manifest = new BundlerManifest();
      manifest.Add("mini_portile2", "2.3.0");
      manifest.Add("nokogiri", "1.8.1");

      var respository = new RubyGemsRepository();

      var calculator = new LibYearCalculator(respository, manifest);

      Assert.Equal(
        0.361,
        calculator.ComputeAsOf(new DateTime(2018, 02, 01)),
        3);
    }

    [Fact]
    public void Compute()
    {
      var olderVersion = new VersionInfo("1.7.0", new DateTime(2016, 12, 27));
      var newerVersion = new VersionInfo("1.7.1", new DateTime(2017, 03, 20));

      var calculator = new LibYearCalculator(null, null);

      Assert.Equal(
        0.227,
        calculator.Compute(olderVersion, newerVersion),
        3);
    }
  }
}
