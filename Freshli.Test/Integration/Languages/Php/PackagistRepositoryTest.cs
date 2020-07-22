using System;
using Freshli.Languages.Php;
using Xunit;

namespace Freshli.Test.Integration {
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

      Assert.Equal(new DateTime(2014, 09, 30), versionInfo.DatePublished);
    }

    [Fact]
    public void LatestAsOf() {
      var phpFixturePath = Fixtures.Path("php", "small");
      var fileFinder = new FileHistoryFinder(phpFixturePath);
      var repository = new MulticastComposerRepository(
        phpFixturePath,
        fileFinder.Finder
      );
      var versionInfo = repository.LatestAsOf(
        "monolog/monolog",
        new DateTime(2020, 01, 01)
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

      var versionInfo = repository.LatestAsOf(
        "symfony/css-selector",
        new DateTime(2020, 01, 01)
      );

      Assert.Equal("v5.0.2", versionInfo.Version);
    }
  }
}
