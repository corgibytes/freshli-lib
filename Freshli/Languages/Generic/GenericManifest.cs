using Freshli.Languages.Generic.Model;
using Newtonsoft.Json;

namespace Freshli.Languages.Generic {
  public class GenericManifest : AbstractManifest {
    public override void Parse(string contents) {
      var manifestModel = JsonConvert.DeserializeObject<ManifestModel>(contents);

      foreach (var dependency in manifestModel.Dependencies) {
        Add(dependency.Name, dependency.ConvertSpecsToString());
      }
    }

    public override bool UsesExactMatches => true;
  }
}
