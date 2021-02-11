using System;
using System.Collections.Generic;
using System.Linq;
using Freshli.Exceptions;

namespace Freshli.Languages.Generic {
  public class GenericRepository : IPackageRepository {
    private IDictionary<string, LinkedList<IVersionInfo>> _repositoryDictionary
      =
      new Dictionary<string, LinkedList<IVersionInfo>>();

    public IVersionInfo VersionInfo(string name, string version) {
      try {
        return _repositoryDictionary[name].First();
      } catch (Exception e) {
        throw new DependencyNotFoundException(name, e);
      }
    }

    public IVersionInfo Latest(
      string name,
      DateTime asOf,
      bool includePreReleases
    ) {
      try {
        return _repositoryDictionary[name].
          First(
            v => includePreReleases == v.IsPreRelease && asOf >= v.DatePublished
          );
      } catch (Exception e) {
        throw new LatestVersionNotFoundException(name, asOf, e);
      }
    }

    public IVersionInfo Latest(string name, DateTime asOf, string thatMatches) {
      throw new NotImplementedException();
    }

    public List<IVersionInfo> VersionsBetween(
      string name,
      DateTime asOf,
      IVersionInfo earlierVersion,
      IVersionInfo laterVersion,
      bool includePreReleases
    ) {
      throw new NotImplementedException();
    }
  }
}
