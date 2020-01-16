using Xunit;

namespace LibMetrics.Test.Integration
{
  public class ManifestFinderTest
  {
    [Fact]
    public void Empty()
    {
      var emptyFixturePath = Fixtures.Path("empty");
      var finder = new ManifestFinder(emptyFixturePath);
      Assert.False(finder.Successful);
    }

    [Fact]
    public void RubyBundler()
    {
      var rubyFixturePath = Fixtures.Path("ruby", "nokotest");

      ManifestFinder.Register<RubyBundlerManifestFinder>();
      var finder = new ManifestFinder(rubyFixturePath);

      Assert.True(finder.Successful);
      Assert.Equal("Gemfile.lock", finder.LockFileName);

      Assert.IsType<RubyGemsRepository>(finder.Calculator.Repository);
      Assert.IsType<BundlerManifest>(finder.Calculator.Manifest);
    }
  }
}
