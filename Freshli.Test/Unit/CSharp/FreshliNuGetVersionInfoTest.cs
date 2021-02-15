using System;
using Freshli.Languages.CSharp;
using Newtonsoft.Json;
using NuGet.Protocol;
using NuGet.Versioning;
using Xunit;

namespace Freshli.Test.Unit.CSharp {
  public class FreshliNuGetVersionInfoTest {
    

    [Theory]
    [InlineData("1", "2", -1)]
    [InlineData("1", "1", 0)]
    [InlineData("2", "1", 1)]
    [InlineData("1.1", "1", 1)]
    [InlineData("1", "1.0.0", 0)]
    [InlineData("2", "1.0.0", 1)]
    [InlineData("2.1", "2.1.0", 0)]
    [InlineData("2.1.0", "2.1.1", -1)]
    public void CompareToCorrectlySortsByVersion(
      string version1,
      string version2,
      int result
    ) {
      var versionInfo1 = new FreshliNuGetVersionInfo(
          new NuGetVersion(version1), DateTime.UtcNow
      );
      var versionInfo2 = new FreshliNuGetVersionInfo(
          new NuGetVersion(version2), DateTime.UtcNow
      );
      
      Assert.Equal(versionInfo1.CompareTo(versionInfo2), result);
    }

    [Theory]
    [InlineData("1.0.0-RC", true)]
    [InlineData("1.0.0", false)]
    public void IdentifiesPreRelease(
      string version,
      bool result
    ) {
      var versionInfo = new FreshliNuGetVersionInfo(
          new NuGetVersion(version), DateTime.UtcNow
      );
      
      Assert.Equal(versionInfo.IsPreRelease, result);
    }

    [Fact]
    public void ProvidesVersion() {
      var version = "1.0.0";
      var versionInfo = new FreshliNuGetVersionInfo(
          new NuGetVersion(version), DateTime.UtcNow
      );
      
      Assert.Equal(versionInfo.Version, version);
    }

    [Fact]
    public void ThrowsExceptionIfNull()
    {
        Assert.Throws<ArgumentException>
        (() => new FreshliNuGetVersionInfo(
            new NuGetVersion("1.0.0"),
            DateTime.UtcNow).CompareTo(null));
    }

    [Fact]
    public void ThrowsExceptionIfNonMatchingType()
    {
        Assert.Throws<ArgumentException>
        (() => new FreshliNuGetVersionInfo(
            new NuGetVersion("1.0.0"),
            DateTime.UtcNow).CompareTo(
                new SemVerVersionInfo("1.0.0")
            )
        );
    }
  }
}
