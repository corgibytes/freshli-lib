using System;

namespace LibMetrics
{
  public class LibYearCalculator
  {
    public IPackageRepository Repository { get; }
    public IManifest Manifest { get; }

    public LibYearCalculator(
      IPackageRepository repository,
      IManifest manifest)
    {
      Repository = repository;
      Manifest = manifest;
    }

    public LibYearResult ComputeAsOf(DateTime date)
    {
      var result = new LibYearResult();

      foreach (var package in Manifest)
      {
        var latestVersion = Repository.LatestAsOf(date, package.Name);
        var currentVersion = Repository.VersionInfo(package.Name, package.Version);
        if (latestVersion != null && currentVersion != null)
        {
          result.Add(
            package.Name,
            latestVersion.Version,
            latestVersion.DatePublished,
            Compute(currentVersion, latestVersion)
          );
        }
      }

      return result;
    }

    public double Compute(VersionInfo olderVersion, VersionInfo newerVersion)
    {
      return (newerVersion.DatePublished - olderVersion.DatePublished).TotalDays / 365.0;
    }
  }
}
