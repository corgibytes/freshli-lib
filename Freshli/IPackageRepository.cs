using System;
using System.Collections.Generic;

namespace Freshli {
  public interface IPackageRepository {
    IVersionInfo VersionInfo(string name, string version);
    IVersionInfo Latest(string name, DateTime asOf);
    IVersionInfo Latest(string name, DateTime asOf, string thatMatches);
    List<IVersionInfo> VersionsBetween(
      string name,
      DateTime asOf,
      IVersionInfo earlierVersion,
      IVersionInfo laterVersion
    );
  }
}
