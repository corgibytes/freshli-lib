using System.IO;
using Freshli.Languages.Php;
using Xunit;

namespace Freshli.Test.Unit {
  public class ComposerManifestTest {
    private static readonly string Contents = File.
      ReadAllText(Fixtures.Path("php", "small", "composer.lock"));

    [Fact]
    public void Parse() {
      var manifest = new ComposerManifest();
      manifest.Parse(Contents);

      AssertManifestContents(manifest);
    }

    [Fact]
    public void ParseImpliesClear() {
      var manifest = new ComposerManifest();
      manifest.Parse(Contents);
      manifest.Add("remove", "me");
      manifest.Parse(Contents);

      AssertManifestContents(manifest);
    }

    private void AssertManifestContents(ComposerManifest manifest) {
      Assert.Equal(15, manifest.Count);
      Assert.Equal("1.3.1", manifest["doctrine/inflector"].Version);
      Assert.Equal("v5.6.39", manifest["illuminate/container"].Version);
      Assert.Equal("v5.6.39", manifest["illuminate/contracts"].Version);
      Assert.Equal("v5.6.39", manifest["illuminate/support"].Version);
      Assert.Equal("1.2.0", manifest["kylekatarnls/update-helper"].Version);
      Assert.Equal("v1.3.0", manifest["laravie/parser"].Version);
      Assert.Equal("1.25.3", manifest["nesbot/carbon"].Version);
      Assert.Equal("v3.6.0", manifest["orchestra/parser"].Version);
      Assert.Equal("1.0.0", manifest["psr/container"].Version);
      Assert.Equal("1.0.1", manifest["psr/simple-cache"].Version);
      Assert.Equal("v1.13.1", manifest["symfony/polyfill-mbstring"].Version);
      Assert.Equal("v4.4.2", manifest["symfony/translation"].Version);
      Assert.Equal("v2.0.1", manifest["symfony/translation-contracts"].Version);
      Assert.Equal("1.1.0", manifest["theseer/tokenizer"].Version);
      Assert.Equal("v1.4.0", manifest["mikey179/vfsstream"].Version);
    }
  }
}
