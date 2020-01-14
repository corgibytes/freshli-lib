using System;

namespace LibMetrics
{
  public interface IPackageRepository
  {
    VersionInfo LatestAsOf(DateTime date, string name);
    VersionInfo VersionInfo(string name, string version);
  }
}