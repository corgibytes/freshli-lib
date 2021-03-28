using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApprovalTests;
using Corgibytes.Freshli.Lib.Languages.Ruby;
using Corgibytes.Freshli.Lib.Util;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration.Languages.Ruby {
  public class RubyGemsRepositoryTest {
    private RubyGemsRepository _repository = new();

    [Fact]
    public void VersionInfoCorrectlyCreatesVersion() {
      var versionInfo = _repository.VersionInfo("tzinfo", "1.2.7");
      var expectedDate =
        new DateTimeOffset(2020, 04, 02, 21, 42, 11, TimeSpan.Zero);

      Assert.Equal("1.2.7", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void VersionInfoCorrectlyCreatesPreReleaseVersion() {
      var versionInfo = _repository.VersionInfo("git", "1.6.0.pre1");
      var expectedDate =
        new DateTimeOffset(2020, 01, 20, 20, 50, 43, TimeSpan.Zero);

      Assert.Equal("1.6.0.pre1", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    private DateTimeOffset BuildDateTimeOffsetFromParts(int[] dateParts) {
      return new(
        dateParts[0],
        dateParts[1],
        dateParts[2],
        dateParts[3],
        dateParts[4],
        dateParts[5],
        TimeSpan.Zero
      );
    }

    [Theory]
    [InlineData(
      new object[] {"git", false},
      new[] {2020, 02, 01, 00, 00, 00},
      "1.5.0",
      new[] {2018, 08, 10, 07, 58, 25}
    )]
    [InlineData(
      new object[] {"git", true},
      new[] {2020, 02, 01, 00, 00, 00},
      "1.6.0.pre1",
      new[] {2020, 01, 20, 20, 50, 43}
    )]
    public void Latest(
      object[] methodParams,
      int[] targetDateParts,
      string expectedVersion,
      int[] expectedDateParts
    ) {
      var gemName = (string) methodParams[0];
      var includePreReleases = (bool) methodParams[1];
      var targetDate = BuildDateTimeOffsetFromParts(targetDateParts);
      var versionInfo = _repository.Latest(
        gemName,
        targetDate,
        includePreReleases
      );
      var expectedDate = BuildDateTimeOffsetFromParts(expectedDateParts);

      Assert.Equal(expectedVersion, versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void VersionsBetweenFindsVersionsReleasedBeforeTargetDate() {
      var targetDate =
        new DateTimeOffset(2014, 04, 01, 00, 00, 00, TimeSpan.Zero);
      var earlierVersion = new RubyGemsVersionInfo {Version = "0.3.38"};
      var laterVersion = new RubyGemsVersionInfo {Version = "1.1.0"};

      var versions = _repository.VersionsBetween(
        "tzinfo",
        targetDate,
        earlierVersion,
        laterVersion,
        includePreReleases: true
      );

      Assert.Equal(3, versions.Count);
    }

    [Fact]
    public void VersionsBetweenCorrectlyFindsVersions() {
      var targetDate =
        new DateTimeOffset(2020, 09, 01, 00, 00, 00, TimeSpan.Zero);
      var earlierVersion = new RubyGemsVersionInfo {Version = "3.11.0"};
      var laterVersion = new RubyGemsVersionInfo {Version = "3.13.0"};

      var versions = _repository.VersionsBetween(name: "google-protobuf",
        targetDate, earlierVersion, laterVersion, includePreReleases: false);

      Assert.Equal(8, versions.Count);
    }


    [Fact]
    public void VersionsBetweenCorrectlyFindsVersionsWithPreReleases() {
      var targetDate =
        new DateTimeOffset(2020, 09, 01, 00, 00, 00, TimeSpan.Zero);
      var earlierVersion = new RubyGemsVersionInfo {Version = "3.11.0"};
      var laterVersion = new RubyGemsVersionInfo {Version = "3.13.0"};

      var versions = _repository.VersionsBetween(name: "google-protobuf",
        targetDate, earlierVersion, laterVersion, includePreReleases: true);

      Assert.Equal(11, versions.Count);
    }
  }
}
