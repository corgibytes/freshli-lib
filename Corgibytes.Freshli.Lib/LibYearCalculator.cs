using System;
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

        public LibYearResult ComputeAsOf(DateTimeOffset date)
        {
            var result = new LibYearResult();

            foreach (var package in Manifest)
            {
                IVersionInfo latestVersion;
                IVersionInfo currentVersion;

                try
                {
                    GetVersions(date, package, out latestVersion, out currentVersion);
                }
                catch (Exception e)
                {
                    HandleFailedPackage(result, package, e);
                    continue;
                }

                if (latestVersion == null || currentVersion == null)
                {
                    continue;
                }

                var packageResult = ProcessPackageResult(
                    date,
                    package,
                    latestVersion,
                    currentVersion
                );

                result.Add(packageResult);
            }

            return result;
        }

        private void GetVersions(
            DateTimeOffset date,
            PackageInfo package,
            out IVersionInfo latestVersion,
            out IVersionInfo currentVersion
        )
        {
            currentVersion = Manifest.UsesExactMatches ?
                        Repository.VersionInfo(package.Name, package.Version) :
                        Repository.Latest(
                            package.Name,
                            asOf: date,
                            thatMatches: package.Version
                        );

            latestVersion =
                Repository.Latest(package.Name, date, currentVersion.IsPreRelease);
        }

        private LibYearPackageResult ProcessPackageResult(
            DateTimeOffset date,
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
                skipped: false
            );

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
                        $"computed for {package.Name} as of {date:O}; " +
                        $"setting value to 0: {packageResult}"
                    );
                }
            }

            _logger.Trace($"PackageResult: {packageResult.ToString()}");
            return packageResult;
        }

        public static double Compute(
            IVersionInfo olderVersion, IVersionInfo newerVersion)
        {
            return (newerVersion.DatePublished - olderVersion.DatePublished).TotalDays / 365.0;
        }

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
                publishedAt: DateTimeOffset.MinValue,
                latestVersion: null,
                latestPublishedAt: DateTimeOffset.MinValue,
                value: 0,
                upgradeAvailable: false,
                skipped: true
            );
            result.Add(packageResult);
            _logger.Trace(e.StackTrace);
        }

        private LibYearPackageResult ComputeUsingVersionsBetween(
            string name,
            DateTimeOffset asOf,
            IVersionInfo currentVersion,
            IVersionInfo latestVersion
        )
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
