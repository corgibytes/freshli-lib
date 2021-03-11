using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib {
  public interface IPackageRepository {
    IVersionInfo VersionInfo(string name, string version);
    IVersionInfo Latest(string name, DateTime asOf, bool includePreReleases);
    IVersionInfo Latest(string name, DateTime asOf, string thatMatches);
    List<IVersionInfo> VersionsBetween(
      string name,
      DateTime asOf,
      IVersionInfo earlierVersion,
      IVersionInfo laterVersion,
      bool includePreReleases
    );
  }
}
