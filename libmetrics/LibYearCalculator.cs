using System;

namespace LibMetrics
{
  public class LibYearCalculator
  {
    private readonly RubyGemsRepository _repository;
    private readonly BundlerManifest _manifest;

    public LibYearCalculator(
      RubyGemsRepository repository,
      BundlerManifest manifest)
    {
      _repository = repository;
      _manifest = manifest;
    }

    public double ComputeAsOf(DateTime date)
    {
      double result = 0;

      foreach (var package in _manifest)
      {
        var latestVersion = _repository.LatestAsOf(date, package.Name);
        var currentVersion = _repository.VersionInfo(package.Name, package.Version);
        result += Compute(currentVersion, latestVersion);
      }

      return result;
    }

    public double Compute(VersionInfo olderVersion, VersionInfo newerVersion)
    {
      return (newerVersion.DatePublished - olderVersion.DatePublished).TotalDays / 365.0;
    }
  }
}
