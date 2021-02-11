using System;
using System.IO;
using Freshli.Languages.Generic.Model;
using Newtonsoft.Json;
using Xunit;

namespace Freshli.Test.Unit.Generic.Model {
  public class ManifestModelTest {
    [Fact]
    public void DeserializeJsonToManifest() {
      var json = File.ReadAllText(@"Unit/Generic/Model/test_manifest.json");
      var manifest = JsonConvert.DeserializeObject<ManifestModel>(json);

      Assert.NotNull(manifest);
      Assert.NotNull(manifest.Dependencies);
      Assert.Equal(27, manifest.Dependencies.Count);
      Assert.Equal(new DateTime(2021, 2, 4), manifest.DateUpdated.Date);
    }
  }
}
