using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IPackageRepository
    {
        // TODO: Create an async version of this method
        IVersionInfo VersionInfo(string name, string version);
        // TODO: Create an async version of this method
        IVersionInfo Latest(string name, DateTime asOf, bool includePreReleases);
        // TODO: Create an async version of this method
        IVersionInfo Latest(string name, DateTime asOf, string thatMatches);
        // TODO: Create an async version of this method
        List<IVersionInfo> VersionsBetween(
          string name,
          DateTime asOf,
          IVersionInfo earlierVersion,
          IVersionInfo laterVersion,
          bool includePreReleases
        );
    }
}
