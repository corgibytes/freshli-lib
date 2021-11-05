using System;
using Corgibytes.Freshli.Lib.Languages.Php;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    public class PackagistRepositoryTest
    {
        public IFileHistoryFinderRegistry FileHistoryFinderRegistry { get; init; }
        public PackagistRepositoryTest()
        {
            FileHistoryFinderRegistry = new FileHistoryFinderRegistry();
            FileHistoryFinderRegistry.Register<GitFileHistoryFinder>();
            FileHistoryFinderRegistry.Register<LocalFileHistoryFinder>();
        }

        [Fact]
        public void VersionInfo()
        {
            var phpFixturePath = Fixtures.Path("php", "small");
            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(phpFixturePath);
            var repository = new MulticastComposerRepository(phpFixturePath, fileFinder);
            var versionInfo = repository.VersionInfo("monolog/monolog", "1.11.0");

            Assert.Equal(new DateTime(2014, 09, 30), versionInfo.DatePublished);
        }

        [Fact]
        public void LatestAsOf()
        {
            var phpFixturePath = Fixtures.Path("php", "small");
            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(phpFixturePath);
            var repository = new MulticastComposerRepository(phpFixturePath, fileFinder);
            var versionInfo = repository.Latest(
              "monolog/monolog",
              new DateTime(2020, 01, 01), includePreReleases: false
            );

            Assert.Equal("2.0.2", versionInfo.Version);
        }

        [Fact]
        public void LatestAsOfForSymfonyCssSelector()
        {
            var phpFixturePath = Fixtures.Path("php", "small");
            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(phpFixturePath);
            var repository = new MulticastComposerRepository(phpFixturePath, fileFinder);

            var versionInfo = repository.Latest(
              "symfony/css-selector",
              new DateTime(2020, 01, 01), includePreReleases: false
            );

            Assert.Equal("v5.0.2", versionInfo.Version);
        }
    }
}
