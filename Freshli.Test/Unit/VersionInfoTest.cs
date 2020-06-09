using System;
using System.Collections.Generic;
using Xunit;

namespace Freshli.Test.Unit
{
  public class VersionInfoTest
  {
    [Theory]
    [InlineData("dev-master", null, null, null, null, null)]
    [InlineData("1", 1, null, null, null, null)]
    [InlineData("v3.6.0", 3, 6, 0, null, null)]
    [InlineData("1.2", 1, 2, null, null, null)]
    [InlineData("1.2.3", 1, 2, 3, null, null)]
    [InlineData("1.2.3a1", 1, 2, 3, "a1", null)]
    [InlineData("1.2.3-a1", 1, 2, 3, "a1", null)]
    [InlineData("1.2.3a1+dev", 1, 2, 3, "a1", "dev")]
    [InlineData("1.2.3-a1+dev", 1, 2, 3, "a1", "dev")]
    public void VersionIsParsedIntoParts(
      string version,
      int? major,
      int? minor,
      int? patch,
      string preRelease,
      string buildMetadata)
    {
      var info = new VersionInfo() { Version = version };
      Assert.Equal(major, info.Major);
      Assert.Equal(minor, info.Minor);
      Assert.Equal(patch, info.Patch);
      Assert.Equal(preRelease, info.PreRelease);
      Assert.Equal(buildMetadata, info.BuildMetadata);
    }

    [Theory]
    [InlineData("1.0.0-alpha", "alpha", null)]
    [InlineData("1.0.0-alpha12", "alpha", 12)]
    [InlineData("1.0.0-alpha.12", "alpha", 12)]
    public void PreReleaseIsParsedIntoParts(string version, string preReleaseLabel, int? preReleaseIncrement)
    {
      var info = new VersionInfo() { Version = version };
      Assert.Equal(preReleaseLabel, info.PreReleaseLabel);
      Assert.Equal(preReleaseIncrement, info.PreReleaseIncrement);
    }

    [Theory]
    [InlineData("1.0.0", "1.0.0", 0)]
    [InlineData("1.10.0", "2.10.0", -1)]
    [InlineData("2.10.0", "1.10.0", 1)]
    [InlineData("1.0.0", "10.0.0", -1)]
    [InlineData("10.0.0", "1.0.0", 1)]
    [InlineData("2.1.0", "2.11.0", -1)]
    [InlineData("2.11.0", "2.1.0", 1)]
    [InlineData("1.9.5", "1.18.5", -1)]
    [InlineData("1.18.5", "1.9.0", 1)]
    [InlineData("8.0.0", "12.0.0", -1)]
    [InlineData("12.0.0", "8.0.0", 1)]
    [InlineData("1.0.0a1", "1.0.0", -1)]
    [InlineData("1.0.0", "1.0.0a1", 1)]
    [InlineData("1.0.0a8", "1.0.0a12", -1)]
    [InlineData("1.0.0a12", "1.0.0a8", 1)]
    [InlineData("1.0.0a.8", "1.0.0a.12", -1)]
    [InlineData("1.0.0a.12", "1.0.0a.8", 1)]
    [InlineData("1.0.0-a.8", "1.0.0-a.12", -1)]
    [InlineData("1.0.0-a.12", "1.0.0-a.8", 1)]
    [InlineData("1.0.0-alpha", "1.0.0-beta", -1)]
    [InlineData("1.0.0-beta", "1.0.0-alpha", 1)]
    [InlineData("1.0.0-alpha", "1.0.0-alpha1", -1)]
    [InlineData("1.0.0-alpha1", "1.0.0-alpha", 1)]
    [InlineData("1.0.0-alpha", "1.0.0", -1)]
    [InlineData("1.0.0", "1.0.0-alpha", 1)]
    [InlineData("1", "1", 0)]
    [InlineData("1.1", "1.1", 0)]
    [InlineData("1.1.1-alpha", "1.1.1-alpha", 0)]
    [InlineData("1", "1.1", -1)]
    [InlineData("1.1", "1", 1)]
    [InlineData("1.1", "1.1.1", -1)]
    [InlineData("1.1.1", "1.1", 1)]
    [InlineData("1.1.1+test", "1.1.1", 0)]
    [InlineData("1.1.1", "1.1.1+test", 0)]
    [InlineData("1.1.1-rc2+dev", "1.1.1-rc2", 0)]
    [InlineData("1.1.1-rc2", "1.1.1-rc2+dev", 0)]
    [InlineData("2", "2.0.0", 0)]
    [InlineData("2.0", "2.0.0", 0)]
    [InlineData("2.0.0", "2", 0)]
    [InlineData("2.0.0", "2.0", 0)]
    [InlineData("2.2", "2.2.0", 0)]
    [InlineData("2.2.0", "2.2", 0)]
    public void CompareToSortsByVersionFirst(string leftVersion, string rightVersion, int expected)
    {
      var left = new VersionInfo() {Version = leftVersion};
      var right = new VersionInfo() {Version = rightVersion};
      Assert.Equal(expected, left.CompareTo(right));
    }

    [Theory]
    [InlineData("1+dev", "1")]
    [InlineData("1.2+dev", "1.2")]
    [InlineData("1.2.3+dev", "1.2.3")]
    [InlineData("1.2.3-alpha1+dev", "1.2.3-alpha1")]
    public void RemovableBuildMetadata(
      string beforeVersion,
      string afterVersion)
    {
      var info = new VersionInfo {Version = beforeVersion};
      info.RemoveBuildMetadata();

      Assert.Equal(new VersionInfo {Version = afterVersion}, info);
      Assert.Equal(afterVersion, info.Version);
    }

    [Theory]
    [InlineData("1", "1")]
    [InlineData("1+dev", "1+dev")]
    [InlineData("1-alpha+dev", "1+dev")]
    [InlineData("1-alpha1+dev", "1+dev")]
    [InlineData("1.2", "1.2")]
    [InlineData("1.2+dev", "1.2+dev")]
    [InlineData("1.2-alpha+dev", "1.2+dev")]
    [InlineData("1.2-alpha1+dev", "1.2+dev")]
    [InlineData("1.2.3", "1.2.3")]
    [InlineData("1.2.3+dev", "1.2.3+dev")]
    [InlineData("1.2.3-alpha+dev", "1.2.3+dev")]
    [InlineData("1.2.3-alpha1+dev", "1.2.3+dev")]
    [InlineData("1.2.3alpha+dev", "1.2.3+dev")]
    [InlineData("1.2.3alpha1+dev", "1.2.3+dev")]
    [InlineData("1.2alpha+dev", "1.2+dev")]
    [InlineData("1.2alpha1+dev", "1.2+dev")]
    [InlineData("1alpha+dev", "1+dev")]
    [InlineData("1alpha1+dev", "1+dev")]
    public void RemovablePreRelease(
      string beforeVersion,
      string afterVersion)
    {
      var info = new VersionInfo {Version = beforeVersion};
      info.RemovePreRelease();

      Assert.Equal(new VersionInfo {Version = afterVersion}, info);
      Assert.Equal(afterVersion, info.Version);
    }

    [Theory]
    [InlineData("1", "1")]
    [InlineData("1+dev", "1+dev")]
    [InlineData("1.2", "1.2")]
    [InlineData("1.2+dev", "1.2+dev")]
    [InlineData("1.2.3", "1.2")]
    [InlineData("1.2.3+dev", "1.2+dev")]
    [InlineData("1.2.3-alpha", "1.2-alpha")]
    [InlineData("1.2.3-alpha+dev", "1.2-alpha+dev")]
    public void RemovablePatch(
      string beforeVersion,
      string afterVersion)
    {
      var info = new VersionInfo {Version = beforeVersion};
      info.RemovePatch();

      Assert.Equal(new VersionInfo {Version = afterVersion}, info);
      Assert.Equal(afterVersion, info.Version);
    }

    [Theory]
    [InlineData("1", "1")]
    [InlineData("1+dev", "1+dev")]
    [InlineData("1.2", "1")]
    [InlineData("1.2+dev", "1+dev")]
    [InlineData("1.2.3", "1")]
    [InlineData("1.2.3+dev", "1+dev")]
    [InlineData("1.2.3-alpha", "1-alpha")]
    [InlineData("1.2.3-alpha+dev", "1-alpha+dev")]
    public void RemovableMinor(
      string beforeVersion,
      string afterVersion)
    {
      var info = new VersionInfo {Version = beforeVersion};
      info.RemoveMinor();

      Assert.Equal(new VersionInfo {Version = afterVersion}, info);
      Assert.Equal(afterVersion, info.Version);
    }
  }
}
