using System;

namespace Freshli {
  public interface IPackageRepository {
    VersionInfo VersionInfo(string name, string version);
    VersionInfo LatestAsOf(string name, DateTime asOf);
    VersionInfo LatestAsOfThatMatches(
      string name,
      DateTime asOf,
      string thatMatches
    );
  }
}
