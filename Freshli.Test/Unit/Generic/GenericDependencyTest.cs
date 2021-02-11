using System;
using System.Collections.Generic;
using System.IO;
using Freshli.Languages.Generic;
using Newtonsoft.Json;
using Xunit;

namespace Freshli.Test.Unit.Generic {
  public class GenericDependencyTest {

    [Fact]
    public void DeserializeJsonToDependency() {
      var json = @"{'name': 'spacy-legacy', 'allows_prerelease': false, 'specs': [{'operator': '<', 'version': '3.1.0'}]}";
      var manifest = JsonConvert.DeserializeObject<GenericDependency>(json);

      Assert.Equal("spacy-legacy", manifest.Name);
      Assert.False(manifest.AllowsPrerelease);
      Assert.Equal(1, manifest.VersionSpecs.Count);

      var manifestVersionSpec = manifest.VersionSpecs[0];
      Assert.Equal(GenericDependency.Operator.LessThan, manifestVersionSpec.Op);
      Assert.Equal("3.1.0", manifestVersionSpec.Version);
    }
  }
}
