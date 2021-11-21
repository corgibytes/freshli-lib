using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Corgibytes.Freshli.Lib.Languages.Ruby;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration.Languages.Ruby
{
    [UsesVerify]
    public class RubyGemsRepositoryProperlyHandlesRateLimitsTest
    {
        [Fact]
        public Task FullSet()
        {
            var repository = new RubyGemsRepository();
            var targetDate = new DateTimeOffset(2021, 03, 10, 00, 00, 00, TimeSpan.Zero);

            var packages = File.ReadAllLines(
                Fixtures.Path("ruby", "rate_limit_packages.txt")
            ).ToList();

            var packagesAndResults = RetrievePackageVersions(
                packages,
                repository,
                targetDate
            );

            return Verifier.Verify(packagesAndResults);
        }

        private static Dictionary<string, List<IVersionInfo>> RetrievePackageVersions(
            List<string> packages,
            RubyGemsRepository repository,
            DateTimeOffset targetDate
        )
        {
            var packagesAndResults = new Dictionary<string, List<IVersionInfo>>();

            packages.ForEach(package =>
            {
                var allVersions = RetrieveGemVersions(repository, targetDate, package);
                packagesAndResults.Add(package, allVersions);
            });

            return packagesAndResults;
        }

        private static List<IVersionInfo> RetrieveGemVersions(
            RubyGemsRepository repository,
            DateTimeOffset targetDate,
            string package
        )
        {
            var latest = repository.Latest(
                package,
                asOf: targetDate,
                includePreReleases: true
            );

            var allVersions = repository.VersionsBetween(
                name: package,
                targetDate,
                RubyGemsVersionInfo.MinimumVersion,
                new RubyGemsVersionInfo(latest.Version, latest.DatePublished),
                includePreReleases: true
            );

            return allVersions;
        }

        // TODO: figure out why this is commented out. Either restore it or delete it
        // private static void VerifyPackageVersions(Dictionary<string, List<IVersionInfo>> packagesAndResults)
        // {
        //     Approvals.VerifyAll(
        //         packagesAndResults,
        //         (package, values) =>
        //         {
        //             var joinedValues = String.Join(
        //                 ", ",
        //                 values.Select(v => v.ToString()).ToList()
        //             );
        //             return $"{package}: [ {joinedValues} ]";
        //         }
        //     );
        // }
    }
}
