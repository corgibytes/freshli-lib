﻿using System;
using Corgibytes.Freshli.Lib.Languages.Perl;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration.Languages.Perl {
  public class MetaCpanRepositoryTest {
    [Fact]
    public void VersionInfoWithoutModuleSeparator() {
      var repository = new MetaCpanRepository();
      var versionInfo = repository.VersionInfo("Plack", "1.0026");
      var expectedDate =
        new DateTimeOffset(2013, 06, 13, 06, 01, 17, TimeSpan.Zero);

      Assert.Equal("1.0026", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void VersionInfoWithModuleSeparator() {
      var repository = new MetaCpanRepository();
      var versionInfo = repository.VersionInfo("Test::More", "1.301001_048");
      var expectedDate =
        new DateTimeOffset(2014, 09, 25, 03, 39, 01, TimeSpan.Zero);

      Assert.Equal("1.301001_048", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void LatestAsOf() {
      var repository = new MetaCpanRepository();
      var targetDate = new DateTimeOffset(2018, 01, 01, 0, 0, 0, TimeSpan.Zero);
      var versionInfo = repository.Latest(
        "Plack", targetDate, includePreReleases: false);
      var expectedDate = new DateTimeOffset(
        2017,
        12,
        31,
        20,
        42,
        50,
        TimeSpan.Zero
      );

      Assert.Equal("1.0045", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Theory]
    [InlineData("Plack", "1.0", 2017, 12, 31, 20, 42, 50, "1.0045")]
    [InlineData("JSON", ">= 2.00, < 2.80", 2013, 10, 17, 11, 03, 11, "2.61")]
    [InlineData(
      "Test::More",
      ">= 0.96, < 2.0",
      2014,
      09,
      26,
      05,
      44,
      26,
      "1.301001_050"
    )]
    public void LatestMatchingVersionExpression(
      string packageName,
      string versionExpression,
      int expectedYear,
      int expectedMonth,
      int expectedDay,
      int expectedHour,
      int expectedMinute,
      int expectedSecond,
      string expectedVersion
    ) {
      var repository = new MetaCpanRepository();
      var targetDate = new DateTimeOffset(2018, 01, 01, 0, 0, 0, TimeSpan.Zero);
      var versionInfo = repository.Latest(
        packageName,
        asOf: targetDate,
        thatMatches: versionExpression
      );
      var expectedDate = new DateTimeOffset(
        expectedYear,
        expectedMonth,
        expectedDay,
        expectedHour,
        expectedMinute,
        expectedSecond,
        TimeSpan.Zero
      );

      Assert.Equal(expectedVersion, versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void VersionsBetween() {
      var repository = new MetaCpanRepository();
      var targetDate =
        new DateTimeOffset(2015, 01, 01, 00, 00, 00, TimeSpan.Zero);
      var earlierVersion = new SemVerVersionInfo("1.0027");
      var laterVersion = new SemVerVersionInfo("1.0045");

      var versions = repository.VersionsBetween("Plack", targetDate,
        earlierVersion, laterVersion, includePreReleases: false);

      Assert.Equal(6, versions.Count);
    }
  }
}
