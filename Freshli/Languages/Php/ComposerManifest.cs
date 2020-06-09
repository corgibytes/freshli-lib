using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Freshli.Languages.Php
{
  public class ComposerManifest: IManifest
  {
    private IList<PackageInfo> _packages = new List<PackageInfo>();
    public IEnumerator<PackageInfo> GetEnumerator()
    {
      return _packages.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int Count => _packages.Count;
    public void Add(string packageName, string packageVersion)
    {
      _packages.Add(new PackageInfo(packageName, packageVersion));
    }

    public void Parse(string contents)
    {
      _packages.Clear();
      var options = new JsonDocumentOptions() {AllowTrailingCommas = true};
      using (var composerData = JsonDocument.Parse(contents, options))
      {
        var packages = composerData.RootElement.GetProperty("packages");
        AddPackagesFromJson(packages);

        var devPackages = composerData.RootElement.GetProperty("packages-dev");
        AddPackagesFromJson(devPackages);
      }
    }

    private void AddPackagesFromJson(JsonElement packages)
    {
      foreach (var package in packages.EnumerateArray())
      {
        var name = package.GetProperty("name").GetString();
        var version = package.GetProperty("version").GetString();
        Add(name, version);
      }
    }

    public PackageInfo this[string packageName]
    {
      get { return _packages.First(package => package.Name == packageName); }
    }

    public bool UsesExactMatches => true;
  }
}
