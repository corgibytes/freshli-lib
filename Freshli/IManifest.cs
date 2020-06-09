using System.Collections.Generic;

namespace Freshli
{
  public interface IManifest : IEnumerable<PackageInfo>
  {
    int Count { get; }
    void Add(string packageName, string packageVersion);
    void Parse(string contents);
    PackageInfo this[string packageName] { get; }
    bool UsesExactMatches { get; }
  }
}
