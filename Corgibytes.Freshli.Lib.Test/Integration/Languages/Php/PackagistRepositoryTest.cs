using System;
using Corgibytes.Freshli.Lib.Languages.Php;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration {
  public class PackagistRepositoryTest {
    [Fact]
    public void VersionInfo() {
      var phpFixturePath = Fixtures.Path("php", "small");
      var fileFinder = new FileHistoryFinder(phpFixturePath);
      var repository = new MulticastComposerRepository(
        phpFixturePath,
        fileFinder.Finder
      );
      var versionInfo = repository.VersionInfo("monolog/monolog", "1.11.0");

      Assert.Equal(
        new DateTimeOffset(2014, 09, 30, 13, 30, 58, TimeSpan.Zero),
        versionInfo.DatePublished
      );
    }

    [Fact]
    public void LatestAsOf() {
      var phpFixturePath = Fixtures.Path("php", "small");
      var fileFinder = new FileHistoryFinder(phpFixturePath);
      var repository = new MulticastComposerRepository(
        phpFixturePath,
        fileFinder.Finder
      );
      var versionInfo = repository.Latest(
        "monolog/monolog",
        new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero),
        includePreReleases: false
      );

      Assert.Equal("2.0.2", versionInfo.Version);
    }

    [Fact]
    public void LatestAsOfForSymfonyCssSelector() {
      var phpFixturePath = Fixtures.Path("php", "small");
      var fileFinder = new FileHistoryFinder(phpFixturePath);
      var repository = new MulticastComposerRepository(
        phpFixturePath,
        fileFinder.Finder
      );

      var versionInfo = repository.Latest(
        "symfony/css-selector",
        new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero),
        includePreReleases: false
      );

      Assert.Equal("v5.0.2", versionInfo.Version);
    }
  }
}
