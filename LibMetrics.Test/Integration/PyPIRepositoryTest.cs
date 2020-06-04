using System;
using LibMetrics.Languages.Python;
using Xunit;

namespace LibMetrics.Test.Integration
{
  public class PyPIRepositoryTest
  {
    [Fact]
    public void VersionInfo()
    {
      var repository = new PyPIRepository();
      var versionInfo = repository.VersionInfo("numpy", "0.9.6");
      var expectedDate = new DateTime(2006, 03, 14, 10, 11, 55, DateTimeKind.Utc);

      Assert.Equal("0.9.6", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void LatestAsOf()
    {
      var repository = new PyPIRepository();
      var targetDate = new DateTime(2020, 01, 01, 0, 0, 0, DateTimeKind.Utc);
      var versionInfo = repository.LatestAsOf(targetDate, "numpy");
      var expectedDate = new DateTime(2019, 12, 22, 15, 32, 32, DateTimeKind.Utc);

      Assert.Equal("1.18.0", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }
  }
}
