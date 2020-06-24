using System;

namespace Freshli.Languages.Perl {
  public class MetaCpanRepository: IPackageRepository {
    public VersionInfo LatestAsOf(DateTime date, string name) {
      throw new NotImplementedException();
    }

    public VersionInfo VersionInfo(string name, string version) {
      throw new NotImplementedException();
    }

    public VersionInfo Latest(string name, string thatMatches, DateTime asOf) {
      throw new NotImplementedException();
    }
  }
}
