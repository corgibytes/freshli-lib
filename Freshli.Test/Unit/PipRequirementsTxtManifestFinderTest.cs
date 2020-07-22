using System.IO;
using Freshli.Languages.Python;
using Xunit;

namespace Freshli.Test.Unit {
  public class PipRequirementsTxtManifestTest {
    private static readonly string Contents = File.
      ReadAllText(
        Fixtures.Path(
          "python",
          "requirements-txt",
          "small",
          "requirements.txt"
        )
      );

    [Fact]
    public void Parse() {
      var manifest = new PipRequirementsTxtManifest();
      manifest.Parse(Contents);

      Assert.Equal(9, manifest.Count);
      Assert.Equal("==1.16.*", manifest["numpy"].Version);
      Assert.Equal("==3.*", manifest["matplotlib"].Version);
      Assert.Equal("==0.8.1", manifest["seaborn"].Version);
      Assert.Equal(">=1.5", manifest["six"].Version);
      Assert.Equal(">1.0.1", manifest["kiwisolver"].Version);
      Assert.Equal("", manifest["pandas"].Version);
      Assert.Equal(">=2017.2,<2020.1", manifest["pytz"].Version);
      Assert.Equal("<1.4.1", manifest["scipy"].Version);
      Assert.Equal("==0.20", manifest["preshed"].Version);
    }

    [Fact]
    public void ParseImpliesClear() {
      var manifest = new PipRequirementsTxtManifest();
      manifest.Parse(Contents);
      manifest.Add("remove", "me");

      manifest.Parse(Contents);

      Assert.Equal(9, manifest.Count);
      Assert.Equal("==1.16.*", manifest["numpy"].Version);
      Assert.Equal("==3.*", manifest["matplotlib"].Version);
      Assert.Equal("==0.8.1", manifest["seaborn"].Version);
      Assert.Equal(">=1.5", manifest["six"].Version);
      Assert.Equal(">1.0.1", manifest["kiwisolver"].Version);
      Assert.Equal("", manifest["pandas"].Version);
      Assert.Equal(">=2017.2,<2020.1", manifest["pytz"].Version);
      Assert.Equal("<1.4.1", manifest["scipy"].Version);
      Assert.Equal("==0.20", manifest["preshed"].Version);
    }

    [Fact]
    public void ParseCorrectlyHandlesPackageNameThatContainsDot() {
      var manifest = new PipRequirementsTxtManifest();
      manifest.Parse("backports.ssl-match-hostname==3.4.0.2");

      Assert.Equal(1, manifest.Count);
      Assert.Equal(
        "==3.4.0.2",
        manifest["backports.ssl-match-hostname"].Version
      );
    }

    [Fact]
    public void ParseRemovesWhiteSpaceFromVersion() {
      var manifest = new PipRequirementsTxtManifest();
      manifest.Parse("preshed == 0.20");

      Assert.Equal(1, manifest.Count);
      Assert.Equal(
        "==0.20",
        manifest["preshed"].Version
      );
    }
  }
}
