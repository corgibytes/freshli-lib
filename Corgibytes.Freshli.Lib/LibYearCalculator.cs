using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Corgibytes.Freshli.Lib
{
    public class LibYearCalculator
    {
        private readonly ILogger<LibYearCalculator> _logger;

        public IPackageRepository Repository { get; }

        private IEnumerable<PackageInfo> packages;
        private bool useExactMatches;

        public LibYearCalculator(
            IPackageRepository repository,
            IEnumerable<PackageInfo> packages,
            bool useExactMatches,
            ILogger<LibYearCalculator> logger
        )
        {
            Repository = repository;
            this.packages = packages;
            this.useExactMatches = useExactMatches;
            _logger = logger;
        }

        public LibYearResult ComputeAsOf(DateTimeOffset date)
        {
            var result = new LibYearResult();

            foreach (var package in packages)
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
            currentVersion = useExactMatches ?
                        Repository.VersionInfo(package.Name, package.Version) :
                        Repository.Latest(
                            package.Name,
                            asOf: date,
                            thatMatches: package.Version
                        );

            latestVersion = null;
            if (currentVersion != null)
            {
                latestVersion = Repository.Latest(package.Name, date, currentVersion.IsPreRelease);
            }
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
                    _logger.LogWarning($"Negative value ({libYearValue:0.000}) " +
                        $"computed for {package.Name} as of {date:O}; " +
                        $"setting value to 0: {packageResult}"
                    );
                }
            }

            _logger.LogTrace($"PackageResult: {packageResult.ToString()}");
            return packageResult;
        }

        public double Compute(
            IVersionInfo olderVersion, IVersionInfo newerVersion)
        {
            return (newerVersion.DatePublished - olderVersion.DatePublished).TotalDays / 365.0;
        }

        private void HandleFailedPackage(
            LibYearResult result,
            PackageInfo package,
            Exception e
        )
        {
            _logger.LogWarning($"Skipping {package.Name}: {e.Message}");
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
            _logger.LogTrace(e.StackTrace);
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
                _logger.LogTrace("Unable to calculate versions between due to language " +
                              "not being implemented, skipping.");
            }

            return null;
        }
    }
}
