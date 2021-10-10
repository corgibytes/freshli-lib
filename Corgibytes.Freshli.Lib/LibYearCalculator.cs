using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace Corgibytes.Freshli.Lib
{
    public class LibYearCalculator
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public IPackageRepository Repository { get; }
        public IManifest Manifest { get; }

        public LibYearCalculator(
          IPackageRepository repository,
          IManifest manifest
        )
        {
            Repository = repository;
            Manifest = manifest;
        }

        // TODO: Convert to asnyc method
        public async Task<LibYearResult> ComputeAsOf(DateTime date)
        {
            var result = new LibYearResult();

            var collectedVersions = new Dictionary<PackageInfo, Task<VersionSet>>();

            foreach (var package in Manifest)
            {
                collectedVersions.Add(package, GetVersions(date, package));
            }

            var tasks = new Task<VersionSet>[collectedVersions.Values.Count];
            collectedVersions.Values.CopyTo(tasks, 0);

            await Task.WhenAll(tasks);

            foreach (var entry in collectedVersions)
            {
                if (entry.Value.IsCompletedSuccessfully)
                {
                    var versionSet = entry.Value.Result;
                    if (versionSet.Latest == null && versionSet.Current == null)
                    {
                        // TODO: Put this in the debug log
                        continue;
                    }
                    result.Add(
                        ProcessPackageResult(date, entry.Key, versionSet.Latest, versionSet.Current)
                    );
                }
                else
                {
                    HandleFailedPackage(result, entry.Key, entry.Value.Exception);
                }
            }

            return result;
        }

        record VersionSet
        {
            internal PackageInfo Package;
            internal IVersionInfo Latest;
            internal IVersionInfo Current;
        }

        private async Task<VersionSet> GetVersions(
            DateTime date,
            PackageInfo package
        )
        {
            var currentVersion = Manifest.UsesExactMatches ?
                (await Repository.VersionInfo(package.Name, package.Version)) :
                Repository.Latest(package.Name,asOf: date,thatMatches: package.Version);

            var latestVersion =
                Repository.Latest(package.Name, date, currentVersion.IsPreRelease);

            return new VersionSet()
            {
                Current = currentVersion,
                Latest = latestVersion,
                Package = package
            };
        }

        // TODO: Convert to async method
        private LibYearPackageResult ProcessPackageResult(
          DateTime date,
          PackageInfo package,
          IVersionInfo latestVersion,
          IVersionInfo currentVersion)
        {
            var libYearValue = Compute(currentVersion, latestVersion);
            var upgradeAvailable = libYearValue > 0;

            var packageResult = new LibYearPackageResult(
              package.Name,
              currentVersion,
              latestVersion,
              Math.Max(0, libYearValue),
              upgradeAvailable,
              skipped: false);

            if (libYearValue < 0)
            {
                var updatedPackageResult = ComputeUsingVersionsBetween(
                    package.Name, date, currentVersion, latestVersion);

                if (updatedPackageResult != null)
                {
                    packageResult = updatedPackageResult;
                }
                else
                {
                    packageResult.UpgradeAvailable = true;
                    _logger.Warn($"Negative value ({libYearValue:0.000}) " +
                                 $"computed for {package.Name} as of {date:d}; " +
                                 $"setting value to 0: {packageResult}");
                }
            }

            _logger.Trace($"PackageResult: {packageResult.ToString()}");
            return packageResult;
        }

        // TODO: Convert to async method
        public static double Compute(
          IVersionInfo olderVersion, IVersionInfo newerVersion)
        {
            return (newerVersion.DatePublished - olderVersion.DatePublished).
              TotalDays / 365.0;
        }

        // TODO: Convert to async method
        private static void HandleFailedPackage(
          LibYearResult result,
          PackageInfo package,
          Exception e
        )
        {
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
        }

        // TODO: Convert to asnyc method
        private LibYearPackageResult ComputeUsingVersionsBetween(string name,
          DateTime asOf, IVersionInfo currentVersion, IVersionInfo latestVersion)
        {
            try
            {
                var versions = Repository.VersionsBetween(
                  name,
                  asOf,
                  currentVersion,
                  latestVersion,
                  currentVersion.IsPreRelease
                );
                foreach (var version in versions)
                {
                    var value = Compute(currentVersion, version);
                    if (value >= 0)
                    {
                        return new LibYearPackageResult(
                          name,
                          currentVersion,
                          version,
                          value,
                          upgradeAvailable: true,
                          skipped: false
                        );
                    }
                }
            }
            catch (NotImplementedException)
            {
                _logger.Trace("Unable to calculate versions between due to language " +
                              "not being implemented, skipping.");
            }

            return null;
        }
    }
}
