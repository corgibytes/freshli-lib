using System.Text.Json;

namespace Freshli.Languages.JavaScript {
  public class PackageJsonManifest: AbstractManifest {
    public override void Parse(string contents) {
      var document = JsonDocument.Parse(contents);

      if (document.RootElement.TryGetProperty(
        "dependencies",
        out var dependencies
      )) {
        ParseDependencies(dependencies);
      }

      if (document.RootElement.TryGetProperty(
        "devDependencies",
        out var devDependencies
      )) {
        ParseDependencies(devDependencies);
      }
    }

    private void ParseDependencies(JsonElement dependencies) {
      foreach (var dependency in dependencies.EnumerateObject()) {
        Add(dependency.Name, dependency.Value.GetString());
      }
    }

    public override bool UsesExactMatches => false;
  }
}
