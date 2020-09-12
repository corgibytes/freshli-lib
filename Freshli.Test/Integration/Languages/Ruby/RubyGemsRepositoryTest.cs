using System;
using Freshli.Languages.Ruby;
using Xunit;

namespace Freshli.Test.Integration.Languages.Ruby {
  public class RubyGemsRepositoryTest {

    [Fact]
    public void VersionInfo() {
      var repository = new RubyGemsRepository();
      var versionInfo = repository.VersionInfo("tzinfo", "1.2.7");
      var expectedDate = new DateTime(2020, 04, 02);

      Assert.Equal("1.2.7", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

      [Fact]
      public void LatestAsOf() {
        var repository = new RubyGemsRepository();
        var targetDate = new DateTime(2020, 01, 01);
        var versionInfo = repository.Latest("tzinfo", targetDate);
        var expectedDate = new DateTime(2019, 12, 24);

        Assert.Equal("2.0.1", versionInfo.Version);
        Assert.Equal(expectedDate, versionInfo.DatePublished);
      }

    [Fact]
    public void VersionsBetween() {
    var repository = new RubyGemsRepository();
    var targetDate = new DateTime(2014, 04, 01);
    var earlierVersion = new RubyGemsVersionInfo {Version = "0.3.38"};
    var laterVersion = new RubyGemsVersionInfo {Version = "1.1.0"};

    var versions = repository.VersionsBetween("tzinfo", targetDate,
      earlierVersion, laterVersion);

    Assert.Equal(3, versions.Count);
    }
  }

}
