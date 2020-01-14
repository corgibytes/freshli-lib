using System.Collections.Generic;

namespace LibMetrics
{
  public interface IManifest : IEnumerable<PackageInfo>
  {
    int Count { get; }
    void Add(string packageName, string packageVersion);
    IEnumerator<PackageInfo> GetEnumerator();
    void Parse(string contents);
    PackageInfo this[string packageName] { get; }
  }
}
