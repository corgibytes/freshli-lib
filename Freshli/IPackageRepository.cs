using System;

namespace Freshli {
  public interface IPackageRepository {
    VersionInfo LatestAsOf(DateTime date, string name);
    VersionInfo VersionInfo(string name, string version);
    VersionInfo Latest(string name, string thatMatches, DateTime asOf);
  }
}
