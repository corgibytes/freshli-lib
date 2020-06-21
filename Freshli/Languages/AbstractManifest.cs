using System.Collections;
using System.Collections.Generic;

namespace Freshli.Languages {
  public abstract class AbstractManifest : IManifest {
    private IDictionary<string, PackageInfo> _packages =
      new Dictionary<string, PackageInfo>();

    public int Count => _packages.Count;
    public abstract bool UsesExactMatches { get; }

    public IEnumerator<PackageInfo> GetEnumerator() {
      return _packages.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public void Add(string packageName, string packageVersion) {
      _packages[packageName] = new PackageInfo(
        packageName,
        packageVersion
      );
    }

    public abstract void Parse(string contents);
    public PackageInfo this[string packageName] => _packages[packageName];
  }
}
