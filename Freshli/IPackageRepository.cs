using System;

namespace Freshli {
  public interface IPackageRepository {
    IVersionInfo VersionInfo(string name, string version);
    IVersionInfo Latest(string name, DateTime asOf);
    IVersionInfo Latest(string name, DateTime asOf, string thatMatches);
  }
}
