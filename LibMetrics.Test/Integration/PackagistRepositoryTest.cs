using System;
using LibMetrics.Languages.Php;
using Xunit;

namespace LibMetrics.Test.Integration
{
  public class PackagistRepositoryTest
  {
    [Fact]
    public void VersionInfo()
    {
      var repository = new PackagistRepository();
      var versionInfo = repository.VersionInfo("monolog/monolog", "1.11.0");

      Assert.Equal(new DateTime(2014, 09, 30), versionInfo.DatePublished);
    }

    [Fact]
    public void LatestAsOf()
    {
      var repository = new PackagistRepository();
      var versionInfo = repository.LatestAsOf(
        new DateTime(2020, 01, 01),
        "monolog/monolog");

      Assert.Equal("2.0.2", versionInfo.Version);
    }
  }
}
