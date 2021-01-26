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
          latestVersion =
            Repository.Latest(package.Name, date, currentVersion.IsPreRelease);
        }
        catch (Exception e) {
          _logger.Warn($"Skipping {package.Name}: {e.Message}");
          var packageResult = new LibYearPackageResult(
            package.Name,
            version: package.Version,
            publishedAt: DateTime.MinValue,
            latestVersion: null,
            latestPublishedAt: DateTime.MinValue,
            value: 0,
            upgradeAvailable: false,
            skipped: true
          );
          result.Add(packageResult);
          _logger.Trace(e.StackTrace);
          continue;
        }

        if (latestVersion != null && currentVersion != null) {
          var libYearValue = Compute(currentVersion, latestVersion);
          var upgradeAvailable = libYearValue > 0;

          var packageResult = new LibYearPackageResult(
            package.Name,
            currentVersion.Version,
            currentVersion.DatePublished,
            latestVersion.Version,
            latestVersion.DatePublished,
            Math.Max(0, libYearValue),
            upgradeAvailable,
            skipped: false);

          if (libYearValue < 0) {
            var updatedPackageResult = ComputeUsingVersionsBetween(
              package.Name, date, currentVersion, latestVersion);

            if (updatedPackageResult != null) {
              packageResult = updatedPackageResult;
            } else {
              packageResult.UpgradeAvailable = true;
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

    public static double Compute(
      IVersionInfo olderVersion, IVersionInfo newerVersion)
    {
      return (newerVersion.DatePublished - olderVersion.DatePublished).
        TotalDays / 365.0;
    }

    private LibYearPackageResult ComputeUsingVersionsBetween(string name,
      DateTime asOf, IVersionInfo currentVersion, IVersionInfo latestVersion)
    {
      try {
        foreach (var version in Repository.VersionsBetween(
          name,
          asOf,
          currentVersion,
          latestVersion,
          currentVersion.IsPreRelease
        )) {
          var value = Compute(currentVersion, version);
          if (value >= 0) {
            return new LibYearPackageResult(
              name,
              currentVersion.Version,
              currentVersion.DatePublished,
              version.Version,
              version.DatePublished,
              value,
              upgradeAvailable: true,
              skipped: false
            );
          }
        }
      } catch (NotImplementedException) {
        return null;
      }
      return null;
    }
  }
}
