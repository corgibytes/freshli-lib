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
          var libYearValue = Compute(currentVersion, latestVersion);

          var packageResult = new LibYearPackageResult(
            package.Name,
            currentVersion.Version,
            currentVersion.DatePublished,
            latestVersion.Version,
            latestVersion.DatePublished,
            Math.Max(0, libYearValue),
            skipped: false);

          if (libYearValue < 0) {
            var updatedPackageResult = ComputeUsingNewerMinorReleases(
              package.Name, currentVersion, latestVersion);

            if (updatedPackageResult != null) {
              packageResult = updatedPackageResult;
            } else {
              _logger.Warn($"Negative value ({libYearValue:0.000}) " +
                $"computed for {package.Name} as of {date:d}; " +
                $"setting value to 0: {packageResult}");
            }
          }

          _logger.Trace(
            $"PackageResult({package.Name}, {package.Version}): " +
            $"{{current = {packageResult.Version}" +
            $"@{packageResult.PublishedAt:d}, " +
            $"latest = {packageResult.LatestVersion}" +
            $"@{packageResult.LatestPublishedAt:d}, " +
            $"value = {packageResult.Value:0.000}}}"
          );

          result.Add(packageResult);
        }
      }

      return result;
    }

    public static double Compute(IVersionInfo olderVersion, IVersionInfo newerVersion)
    {
      return (newerVersion.DatePublished - olderVersion.DatePublished).
        TotalDays / 365.0;
    }

    private LibYearPackageResult ComputeUsingNewerMinorReleases(string name,
      IVersionInfo currentVersion, IVersionInfo latestVersion)
    {
      foreach (var version in Repository.VersionsBetween(
        name, currentVersion, latestVersion)) {
        var value = Compute(currentVersion, version);
        if (value >= 0) {
          return new LibYearPackageResult(
            name,
            currentVersion.Version,
            currentVersion.DatePublished,
            version.Version,
            version.DatePublished,
            value,
            skipped: false);
        }
      }
      return null;
    }
  }
}
