using System;
using Freshli.Languages.Ruby;
using Xunit;

namespace Freshli.Test.Integration.Languages.Ruby {
  public class RubyGemsRepositoryTest {

    [Fact]
    public void VersionInfoCorrectlyCreatesVersion() {
      var repository = new RubyGemsRepository();
      var versionInfo = repository.VersionInfo("tzinfo", "1.2.7");
      var expectedDate = new DateTime(2020, 04, 02);

      Assert.Equal("1.2.7", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void VersionInfoCorrectlyCreatesPreReleaseVersion() {
      var repository = new RubyGemsRepository();
      var versionInfo = repository.VersionInfo("git", "1.6.0.pre1");
      var expectedDate = new DateTime(2020, 01, 20);

      Assert.Equal("1.6.0.pre1", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void LatestAsOfCorrectlyFindsLatestVersion() {
      var repository = new RubyGemsRepository();
      var targetDate = new DateTime(2020, 02, 01);
      var versionInfo = repository.Latest("git", targetDate, false);
      var expectedDate = new DateTime(2018, 08, 10);

      Assert.Equal("1.5.0", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void
      LatestAsOfCorrectlyFindsLatestPreReleaseVersion() {
      var repository = new RubyGemsRepository();
      var targetDate = new DateTime(2020, 02, 01);
      var versionInfo = repository.Latest("git", targetDate, true);
      var expectedDate = new DateTime(2020, 01, 20);

      Assert.Equal("1.6.0.pre1", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void VersionsBetweenFindsVersionsReleasedBeforeTargetDate() {
    var repository = new RubyGemsRepository();
    var targetDate = new DateTime(2014, 04, 01);
    var earlierVersion = new RubyGemsVersionInfo {Version = "0.3.38"};
    var laterVersion = new RubyGemsVersionInfo {Version = "1.1.0"};

    var versions = repository.VersionsBetween("tzinfo", targetDate,
      earlierVersion, laterVersion, includePreReleases: true);

    Assert.Equal(3, versions.Count);
    }

    [Fact]
    public void VersionsBetweenCorrectlyFindsVersions() {
      var repository = new RubyGemsRepository();
      var targetDate = new DateTime(2020, 09, 01);
      var earlierVersion = new RubyGemsVersionInfo {Version = "3.11.0"};
      var laterVersion = new RubyGemsVersionInfo {Version = "3.13.0"};

      var versions = repository.VersionsBetween(name: "google-protobuf",
        targetDate, earlierVersion, laterVersion, includePreReleases: false);

      Assert.Equal(8, versions.Count);
    }


    [Fact]
    public void VersionsBetweenCorrectlyFindsVersionsWithPreReleases() {
      var repository = new RubyGemsRepository();
      var targetDate = new DateTime(2020, 09, 01);
      var earlierVersion = new RubyGemsVersionInfo {Version = "3.11.0"};
      var laterVersion = new RubyGemsVersionInfo {Version = "3.13.0"};

      var versions = repository.VersionsBetween(name: "google-protobuf",
        targetDate, earlierVersion, laterVersion, includePreReleases: true);

      Assert.Equal(11, versions.Count);
    }
  }
}
