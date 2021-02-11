using System.IO;
using Freshli.Languages.Generic;
using Xunit;

namespace Freshli.Test.Unit.Generic {
  public class GenericManifestTest {
    [Fact]
    public void Parse() {
      var json = File.ReadAllText(@"Unit/Generic/Model/test_manifest.json");
      var manifest = new GenericManifest();

      manifest.Parse(json);

      Assert.Equal(27, manifest.Count);
    }
  }
}
