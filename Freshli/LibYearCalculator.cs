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
          latestVersion = Repository.Latest(package.Name, date);

          if (Manifest.UsesExactMatches) {
            currentVersion =
              Repository.VersionInfo(package.Name, package.Version);
          } else {
            currentVersion = Repository.Latest(
              package.Name,
              asOf: date,
              thatMatches: package.Version
            );
          }
        }
        catch (Exception e) {
          _logger.Warn($"Skipping {package.Name}: {e.Message}");
          result.Add(
            package.Name,
            version: package.Version,
            publishedAt: DateTime.MinValue,
            latestVersion: null,
            latestPublishedAt: DateTime.MinValue,
            value: 0,
            skipped: true);
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

          var value = Compute(currentVersion, latestVersion);
          var skipped = value < 0;

          var packageResult = new LibYearPackageResult(
            package.Name,
            currentVersion.Version,
            currentVersion.DatePublished,
            latestVersion.Version,
            latestVersion.DatePublished,
            value,
            skipped); //TODO: Should these be skipped?

          if (skipped) {
            _logger.Warn($"Skipping {package.Name}: Negative value " +
              $"computed as of {date:d}: {packageResult}");
          }
          result.Add(packageResult);
        }
      }

      return result;
    }

    public double Compute(IVersionInfo olderVersion, IVersionInfo newerVersion)
    {
      return (newerVersion.DatePublished - olderVersion.DatePublished).
        TotalDays / 365.0;
    }
  }
}
