using System.Text.Json;

namespace Freshli.Languages.JavaScript {
  public class PackageLockJsonManifest: AbstractManifest {
    public override void Parse(string contents) {
      var document = JsonDocument.Parse(contents);

      if (document.RootElement.TryGetProperty(
        "dependencies",
        out var dependencies
      )) {
        foreach (var dependency in dependencies.EnumerateObject()) {
          if (dependency.Value.TryGetProperty("version", out var versionNode)) {
            Add(dependency.Name, versionNode.GetString());
          }
        }
      }
    }

    public override bool UsesExactMatches => true;
  }
}
