using System;
using Freshli.Languages.Ruby;
using Xunit;

namespace Freshli.Test.Integration {
  public class LibYearCalculatorTest {
    [Fact]
    public void ComputeAsOf() {
      var manifest = new BundlerManifest();
      manifest.Add("mini_portile2", "2.1.0");
      manifest.Add("nokogiri", "1.7.0");

      var repository = new RubyGemsRepository();

      var calculator = new LibYearCalculator(repository, manifest);

      var results = calculator.ComputeAsOf(new DateTime(2017, 04, 01));

      Assert.Equal(0.227, results.Total, 3);

      Assert.Equal(0.0, results["mini_portile2"].Value, 3);
      Assert.Equal("2.1.0", results["mini_portile2"].Version);
      Assert.Equal(
        new DateTime(2016, 01, 06),
        results["mini_portile2"].PublishedAt
      );
      Assert.Equal(0.227, results["nokogiri"].Value, 3);
      Assert.Equal("1.7.1", results["nokogiri"].Version);
      Assert.Equal(new DateTime(2017, 03, 20), results["nokogiri"].PublishedAt);
    }

    [Fact]
    public void ComputeAsOfSmallValue() {
      var manifest = new BundlerManifest();
      manifest.Add("mini_portile2", "2.1.0");
      manifest.Add("nokogiri", "1.7.0");

      var repository = new RubyGemsRepository();

      var calculator = new LibYearCalculator(repository, manifest);

      var results = calculator.ComputeAsOf(new DateTime(2017, 02, 01));

      Assert.Equal(0.022, results.Total, 3);

      Assert.Equal(0, results["mini_portile2"].Value, 3);
      Assert.Equal("2.1.0", results["mini_portile2"].Version);
      Assert.Equal(0.022, results["nokogiri"].Value, 3);
      Assert.Equal("1.7.0.1", results["nokogiri"].Version);
    }

    [Fact]
    public void ComputeAsOfEdgeCase() {
      var manifest = new BundlerManifest();
      manifest.Add("mini_portile2", "2.3.0");
      manifest.Add("nokogiri", "1.8.1");

      var respository = new RubyGemsRepository();

      var calculator = new LibYearCalculator(respository, manifest);

      var results = calculator.ComputeAsOf(new DateTime(2018, 01, 01));

      Assert.Equal(0, results.Total, 3);

      Assert.Equal(0, results["mini_portile2"].Value, 3);
      Assert.Equal("2.3.0", results["mini_portile2"].Version);
      Assert.Equal(0, results["nokogiri"].Value, 3);
      Assert.Equal("1.8.1", results["nokogiri"].Version);
    }

    [Fact]
    public void ComputeAsOfOtherEdgeCase() {
      var manifest = new BundlerManifest();
      manifest.Add("mini_portile2", "2.3.0");
      manifest.Add("nokogiri", "1.8.1");

      var respository = new RubyGemsRepository();

      var calculator = new LibYearCalculator(respository, manifest);

      var results = calculator.ComputeAsOf(new DateTime(2018, 02, 01));

      Assert.Equal(0.362, results.Total, 3);

      Assert.Equal(0.0, results["mini_portile2"].Value, 3);
      Assert.Equal("2.3.0", results["mini_portile2"].Version);
      Assert.Equal(0.362, results["nokogiri"].Value, 3);
      Assert.Equal("1.8.2", results["nokogiri"].Version);
    }

    [Fact]
    public void Compute() {
      var olderVersion = new SemVerVersionInfo("1.7.0",
        new DateTime(2016, 12, 27));
      var newerVersion = new SemVerVersionInfo("1.7.1",
        new DateTime(2017, 03, 20));

      var calculator = new LibYearCalculator(null, null);

      Assert.Equal(
        0.227,
        calculator.Compute(olderVersion, newerVersion),
        3
      );
    }
  }
}
