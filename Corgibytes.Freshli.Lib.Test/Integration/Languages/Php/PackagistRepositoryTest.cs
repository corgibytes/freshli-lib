using System;
using Corgibytes.Freshli.Lib.Languages.Php;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    public class PackagistRepositoryTest
    {
        [Fact]
        public async void VersionInfo()
        {
            var phpFixturePath = Fixtures.Path("php", "small");
            var fileFinder = new FileHistoryFinder(phpFixturePath);
            var repository = new MulticastComposerRepository(
              phpFixturePath,
              fileFinder.Finder
            );
            var versionInfo = await repository.VersionInfo("monolog/monolog", "1.11.0");

            Assert.Equal(new DateTime(2014, 09, 30), versionInfo.DatePublished);
        }

        [Fact]
        public async void LatestAsOf()
        {
            var phpFixturePath = Fixtures.Path("php", "small");
            var fileFinder = new FileHistoryFinder(phpFixturePath);
            var repository = new MulticastComposerRepository(
              phpFixturePath,
              fileFinder.Finder
            );
            var versionInfo = await repository.Latest(
              "monolog/monolog",
              new DateTime(2020, 01, 01), includePreReleases: false
            );

            Assert.Equal("2.0.2", versionInfo.Version);
        }

        [Fact]
        public async void LatestAsOfForSymfonyCssSelector()
        {
            var phpFixturePath = Fixtures.Path("php", "small");
            var fileFinder = new FileHistoryFinder(phpFixturePath);
            var repository = new MulticastComposerRepository(
              phpFixturePath,
              fileFinder.Finder
            );

            var versionInfo = await repository.Latest(
              "symfony/css-selector",
              new DateTime(2020, 01, 01), includePreReleases: false
            );

            Assert.Equal("v5.0.2", versionInfo.Version);
        }
    }
}
