using System;
using Corgibytes.Freshli.Lib.Languages.Python;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration.Languages.Python {
  public class PyPIRepositoryTest {
    [Fact]
    public void VersionInfo() {
      var repository = new PyPIRepository();
      var versionInfo = repository.VersionInfo("numpy", "0.9.6");
      var expectedDate = new DateTimeOffset(
        2006,
        03,
        14,
        10,
        11,
        55,
        TimeSpan.Zero
      );

      Assert.Equal("0.9.6", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void LatestAsOf() {
      var repository = new PyPIRepository();
      var targetDate =
        new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero);
      var versionInfo = repository.Latest(
        "numpy", targetDate, includePreReleases: false);
      var expectedDate = new DateTimeOffset(
        2019,
        12,
        22,
        15,
        32,
        32,
        TimeSpan.Zero
      );

      Assert.Equal("1.18.0", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Theory]
    [InlineData("numpy", "==1.16.*", 2019, 12, 29, 22, 23, 23, "1.16.6")]
    [InlineData("matplotlib", "==3.*", 2019, 11, 21, 22, 51, 38, "3.1.2")]
    [InlineData("seaborn", "==0.8.1", 2017, 09, 03, 16, 38, 23, "0.8.1")]
    public void VersionExpressionMatching(
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
      var targetDate =
        new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero);

      var repository = new PyPIRepository();
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
    var repository = new PyPIRepository();
    var targetDate = new DateTimeOffset(2015, 12, 01, 0, 0, 0, TimeSpan.Zero);
    var earlierVersion = new PythonVersionInfo("2.9");
    var laterVersion = new PythonVersionInfo("3.0.3");

    var versions = repository.VersionsBetween("pymongo", targetDate,
      earlierVersion, laterVersion, includePreReleases: false);

    Assert.Equal(4, versions.Count);
    }
  }

}
