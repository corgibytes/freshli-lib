using Freshli.Languages.Generic;
using Freshli.Languages.Generic.Model;
using Newtonsoft.Json;
using Xunit;

namespace Freshli.Test.Unit.Generic.Model {
  public class DependencyModelTest {

    [Fact]
    public void DeserializeJsonToDependency() {
      var json = @"{'name': 'spacy-legacy', 'allows_prerelease': false, 'specs': [{'operator': '<', 'version': '3.1.0'}]}";
      var manifest = JsonConvert.DeserializeObject<DependencyModel>(json);

      Assert.Equal("spacy-legacy", manifest.Name);
      Assert.False(manifest.AllowsPrerelease);
      Assert.Equal(1, manifest.VersionSpecs.Count);

      var manifestVersionSpec = manifest.VersionSpecs[0];
      Assert.Equal(GenericOperator.Operator.LessThan, manifestVersionSpec.Op);
      Assert.Equal("3.1.0", manifestVersionSpec.Version);
    }
  }
}
