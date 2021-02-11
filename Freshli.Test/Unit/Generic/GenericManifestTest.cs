using System;
using System.Collections.Generic;
using System.IO;
using Freshli.Languages.Generic;
using Newtonsoft.Json;
using Xunit;

namespace Freshli.Test.Unit.Generic {
  public class GenericManifestTest {
    [Fact]
    public void DeserializeJsonToManifest() {
      var json = File.ReadAllText(@"Unit/Generic/test_manifest.json");
      var manifest = JsonConvert.DeserializeObject<GenericManifest>(json);

      Assert.NotNull(manifest);
      Assert.NotNull(manifest.Dependencies);
      Assert.Equal(27, manifest.Dependencies.Count);
      Assert.Equal(new DateTime(2021, 2, 4), manifest.DateUpdated.Date);
    }
  }
}
