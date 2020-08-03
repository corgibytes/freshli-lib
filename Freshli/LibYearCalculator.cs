using System;
using NLog;

namespace Freshli {
  public class LibYearCalculator {
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    public IPackageRepository Repository { get; }
    public IManifest Manifest { get; }

    public LibYearCalculator(
      IPackageRepository repository,
      IManifest manifest
    ) {
      Repository = repository;
      Manifest = manifest;
    }

    public LibYearResult ComputeAsOf(DateTime date) {
      var result = new LibYearResult();

      foreach (var package in Manifest) {
        IVersionInfo latestVersion;
        IVersionInfo currentVersion;

        try {
          latestVersion = Repository.LatestAsOf(package.Name, date);

          if (Manifest.UsesExactMatches) {
            currentVersion =
              Repository.VersionInfo(package.Name, package.Version);
          } else {
            currentVersion = Repository.LatestAsOfThatMatches(
              package.Name,
              asOf: date,
              thatMatches: package.Version
            );
          }
        }
        catch (Exception e) {
          _logger.Warn($"Skipping {package.Name}: {e.Message}");
          result.Add(package.Name, package.Version, DateTime.MinValue,
            0, true);
          _logger.Trace(e.StackTrace);
          continue;
        }

        if (latestVersion != null && currentVersion != null) {
          _logger.Trace(
            $"Package({package.Name}, {package.Version}): " +
            $"current = {currentVersion.ToSimpleVersion()}" +
            $"@{currentVersion.DatePublished:d}, " +
            $"latest = {latestVersion.ToSimpleVersion()}" +
            $"@{latestVersion.DatePublished:d}"
          );
          result.Add(
            package.Name,
            latestVersion.Version,
            latestVersion.DatePublished,
            Compute(currentVersion, latestVersion),
            false
          );
        }
      }

      return result;
    }

    public double Compute(IVersionInfo olderVersion, IVersionInfo newerVersion) {
      return (newerVersion.DatePublished - olderVersion.DatePublished).
        TotalDays / 365.0;
    }
  }
}
