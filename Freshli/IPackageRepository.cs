using System;

namespace Freshli {
  public interface IPackageRepository {
    IVersionInfo VersionInfo(string name, string version);
    IVersionInfo LatestAsOf(string name, DateTime asOf);
    IVersionInfo LatestAsOfThatMatches(
      string name,
      DateTime asOf,
      string thatMatches
    );
  }
}
