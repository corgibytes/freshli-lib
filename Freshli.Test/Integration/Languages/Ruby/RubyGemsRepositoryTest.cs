using System;
using System.Linq;
using Freshli.Languages.Ruby;
using Freshli.Util;
using Xunit;

namespace Freshli.Test.Integration.Languages.Ruby {
  public class RubyGemsRepositoryTest {

    [Fact]
    public void VersionInfoCorrectlyCreatesVersion() {
      var repository = new RubyGemsRepository();
      var versionInfo = repository.VersionInfo("tzinfo", "1.2.7");
      var expectedDate =
        new DateTimeOffset(2020, 04, 02, 21, 42, 11, TimeSpan.Zero);

      Assert.Equal("1.2.7", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void VersionInfoCorrectlyCreatesPreReleaseVersion() {
      var repository = new RubyGemsRepository();
      var versionInfo = repository.VersionInfo("git", "1.6.0.pre1");
      var expectedDate =
        new DateTimeOffset(2020, 01, 20, 20, 50, 43, TimeSpan.Zero);

      Assert.Equal("1.6.0.pre1", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void LatestAsOfCorrectlyFindsLatestVersion() {
      var repository = new RubyGemsRepository();
      var targetDate =
        new DateTimeOffset(2020, 02, 01, 00, 00, 00, TimeSpan.Zero);
      var versionInfo = repository.Latest("git", targetDate, false);
      var expectedDate =
        new DateTimeOffset(2018, 08, 10, 07, 58, 25, TimeSpan.Zero);

      Assert.Equal("1.5.0", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void
      LatestAsOfCorrectlyFindsLatestPreReleaseVersion() {
      var repository = new RubyGemsRepository();
      var targetDate =
        new DateTimeOffset(2020, 02, 01, 00, 00, 00, TimeSpan.Zero);
      var versionInfo = repository.Latest("git", targetDate, true);
      var expectedDate =
        new DateTimeOffset(2020, 01, 20, 20, 50, 43, TimeSpan.Zero);

      Assert.Equal("1.6.0.pre1", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void VersionsBetweenFindsVersionsReleasedBeforeTargetDate() {
    var repository = new RubyGemsRepository();
    var targetDate =
      new DateTimeOffset(2014, 04, 01, 00, 00, 00, TimeSpan.Zero);
    var earlierVersion = new RubyGemsVersionInfo {Version = "0.3.38"};
    var laterVersion = new RubyGemsVersionInfo {Version = "1.1.0"};

    var versions = repository.VersionsBetween("tzinfo", targetDate,
      earlierVersion, laterVersion, includePreReleases: true);

    Assert.Equal(3, versions.Count);
    }

    [Fact]
    public void VersionsBetweenCorrectlyFindsVersions() {
      var repository = new RubyGemsRepository();
      var targetDate =
        new DateTimeOffset(2020, 09, 01, 00, 00, 00, TimeSpan.Zero);
      var earlierVersion = new RubyGemsVersionInfo {Version = "3.11.0"};
      var laterVersion = new RubyGemsVersionInfo {Version = "3.13.0"};

      var versions = repository.VersionsBetween(name: "google-protobuf",
        targetDate, earlierVersion, laterVersion, includePreReleases: false);

      Assert.Equal(8, versions.Count);
    }


    [Fact]
    public void VersionsBetweenCorrectlyFindsVersionsWithPreReleases() {
      var repository = new RubyGemsRepository();
      var targetDate =
        new DateTimeOffset(2020, 09, 01, 00, 00, 00, TimeSpan.Zero);
      var earlierVersion = new RubyGemsVersionInfo {Version = "3.11.0"};
      var laterVersion = new RubyGemsVersionInfo {Version = "3.13.0"};

      var versions = repository.VersionsBetween(name: "google-protobuf",
        targetDate, earlierVersion, laterVersion, includePreReleases: true);

      Assert.Equal(11, versions.Count);
    }

    [Fact]
    public void ProperlyHandlesRateLimits() {
      var repository = new RubyGemsRepository();
      var targetDate =
        new DateTimeOffset(2021, 03, 10, 00, 00, 00, TimeSpan.Zero);

      var packages = new[] {
        ("bundler", "2.2.13"),
        ("rspec-mocks", "3.10.2"),
        ("rspec-support", "3.10.2"),
        ("i18n", "1.8.9"),
        ("activesupport", "6.1.3"),
        ("aws-sdk-core", "3.112.1"),
        ("rubygems-update", "3.2.13"),
        ("thor", "1.10.0"),
        ("nokogiri", "1.11.1"),
        ("minitest", "5.14.4"),
        ("aws-sigv4", "1.2.3"),
        ("faraday", "1.3.0"),
        ("concurrent-ruby", "1.1.8"),
        ("ffi", "1.15.0"),
        ("activemodel", "6.1.3"),
        ("activerecord", "6.1.3"),
        ("actionpack", "6.1.3"),
        ("rails", "6.1.3"),
        ("aws-sdk", "3.0.2"),
        ("actionmailer", "6.1.3"),
        ("railties", "6.1.3"),
        ("aws-partitions", "1.431.1"),
        ("aws-eventstream", "1.1.1"),
        ("aws-sdk-resources", "3.94.1"),
        ("actionview", "6.1.3"),
        ("mime-types-data", "3.2021.0225"),
        ("loofah", "2.9.0"),
        ("activejob", "6.1.3"),
        ("pry", "0.14.0"),
        ("excon", "0.79.0"),
        ("aws-sdk-s3", "1.89.0")
      }.ToList();

      packages.ForEach(
        packageAndVersion => {
          var latest = repository.Latest(
            packageAndVersion.Item1,
            asOf: targetDate,
            true
          );

          var allVersions = repository.VersionsBetween(
            name: packageAndVersion.Item1,
            targetDate,
            RubyGemsVersionInfo.MinimumVersion,
            new RubyGemsVersionInfo(latest.Version, latest.DatePublished),
            includePreReleases: true
          );

          Assert.NotEmpty(allVersions);
        }
      );
    }
  }
}
