using Xunit;

namespace LibMetrics.Test.Unit
{
  public class BundlerManifestTest
  {
    private static readonly string Contents = @"GEM
  remote: https://rubygems.org/
  specs:
    mini_portile2 (2.4.0)
    nokogiri (1.9.1)
      mini_portile2 (~> 2.4.0)

PLATFORMS
  ruby

DEPENDENCIES
  nokogiri (= 1.9.1)

BUNDLED WITH
   2.1.3
";

    [Fact]
    public void Parse()
    {
      var manifest = new BundlerManifest();
      manifest.Parse(Contents);

      Assert.Equal(2, manifest.Count);
      Assert.Equal("2.4.0", manifest["mini_portile2"].Version);
      Assert.Equal("1.9.1", manifest["nokogiri"].Version);
    }

    [Fact]
    public void DoubleParse()
    {
      var manifest = new BundlerManifest();
      manifest.Parse(Contents);
      manifest.Parse(Contents);

      Assert.Equal(2, manifest.Count);
      Assert.Equal("2.4.0", manifest["mini_portile2"].Version);
      Assert.Equal("1.9.1", manifest["nokogiri"].Version);
    }
  }
}
