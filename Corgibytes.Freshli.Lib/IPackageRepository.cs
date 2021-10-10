using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Corgibytes.Freshli.Lib
{
    public interface IPackageRepository
    {
        Task<IVersionInfo> VersionInfo(string name, string version);
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
