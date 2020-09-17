using System;
using Freshli.Exceptions;
using Freshli.Languages.Python;
using Xunit;

namespace Freshli.Test.Unit.Python {
  public class PythonVersionInfoTest {

    [Theory]
    [InlineData("1!1", 1, "1", new long[] { 1 }, null, null, null, null, null)]
    [InlineData
      ("10!1", 10, "1", new long[] { 1 }, null, null, null, null, null)]
    [InlineData
      ("10!1.0", 10, "1.0", new long[] { 1, 0 }, null, null, null, null, null)]
    [InlineData("20200101!1",
      20200101, "1", new long[] { 1 }, null, null, null, null, null)]
    [InlineData
      ("0.1", 0, "0.1", new long[] { 0, 1 }, null, null, null, null, null)]
    [InlineData
      ("0.10", 0, "0.10", new long[] { 0, 10 }, null, null, null, null, null)]
    [InlineData("0.10.1",
      0, "0.10.1", new long[] { 0, 10, 1 }, null, null, null, null, null)]
    [InlineData
      ("1.0", 0, "1.0", new long[] { 1, 0 }, null, null, null, null, null)]
    [InlineData
      ("10.0", 0, "10.0", new long[] { 10, 0 }, null, null, null, null, null)]
    [InlineData("10.0.1.0",
      0, "10.0.1.0", new long[] { 10, 0, 1, 0 }, null, null, null, null, null)]
    [InlineData("1.0.dev456",
      0, "1.0", new long[] { 1, 0 }, null, null, null, null, 456)]
    [InlineData
      ("1.0a1", 0, "1.0", new long[] { 1, 0 }, "a", 1, null, null, null)]
    [InlineData
      ("1.0A1", 0, "1.0", new long[] { 1, 0 }, "a", 1, null, null, null)]
    [InlineData
      ("1.0.1a1", 0, "1.0.1", new long[] { 1, 0, 1 }, "a", 1, null, null, null)]
    [InlineData
      ("1.0a2.dev456", 0, "1.0", new long[] { 1, 0 }, "a", 2, null, null, 456)]
    [InlineData("1.0a12.dev456",
      0, "1.0", new long[] { 1, 0 }, "a", 12, null, null, 456)]
    [InlineData("1.0a12.post456",
      0, "1.0", new long[] { 1, 0 }, "a", 12, "post", 456, null)]
    [InlineData
      ("1.0a12", 0, "1.0", new long[] { 1, 0 }, "a", 12, null, null, null)]
    [InlineData
      ("1.0b1.dev456", 0, "1.0", new long[] { 1, 0 }, "b", 1, null, null, 456)]
    [InlineData
      ("1.0b2", 0, "1.0", new long[] { 1, 0 }, "b", 2, null, null, null)]
    [InlineData("1.0b2.post345.dev456",
      0, "1.0", new long[] { 1, 0 }, "b", 2, "post", 345, 456)]
    [InlineData("1.0b2.post345",
      0, "1.0", new long[] { 1, 0 }, "b", 2, "post", 345, null)]
    [InlineData
      ("1.0rc1", 0, "1.0", new long[] { 1, 0 }, "rc", 1, null, null, null)]
    [InlineData("1.0rc1.dev456",
      0, "1.0", new long[] { 1, 0 }, "rc", 1, null, null, 456)]
    [InlineData("1.0rc1.post456",
      0, "1.0", new long[] { 1, 0 }, "rc", 1, "post", 456, null)]
    [InlineData("1.0alpha1",
      0, "1.0", new long[] { 1, 0 }, "alpha", 1, null, null, null)]
    [InlineData
      ("1.0beta1", 0, "1.0", new long[] { 1, 0 }, "beta", 1, null, null, null)]
    [InlineData
      ("1.0pre1", 0, "1.0", new long[] { 1, 0 }, "pre", 1, null, null, null)]
    [InlineData("1.0preview1",
      0, "1.0", new long[] { 1, 0 }, "preview", 1, null, null, null)]
    [InlineData("1.0.post456.dev34",
      0, "1.0", new long[] { 1, 0 }, null, null, "post", 456, 34)]
    [InlineData("1.0.post456",
      0, "1.0", new long[] { 1, 0 }, null, null, "post", 456, null)]
    [InlineData("1.0.POST456",
      0, "1.0", new long[] { 1, 0 }, null, null, "post", 456, null)]
    [InlineData("1.0.rev456",
      0, "1.0", new long[] { 1, 0 }, null, null, "rev", 456, null)]
    [InlineData("1.0.REV456",
      0, "1.0", new long[] { 1, 0 }, null, null, "rev", 456, null)]
    [InlineData
      ("1.0.r456", 0, "1.0", new long[] { 1, 0 }, null, null, "r", 456, null)]
    [InlineData
      ("1.0.R456", 0, "1.0", new long[] { 1, 0 }, null, null, "r", 456, null)]
    [InlineData
      ("1.1.dev1", 0, "1.1", new long[] { 1, 1 }, null, null, null, null, 1)]
    [InlineData
      ("1.1.DEV1", 0, "1.1", new long[] { 1, 1 }, null, null, null, null, 1)]
    [InlineData("2020!1.0.1.2.3.4b2.post345.dev456", 2020, "1.0.1.2.3.4",
      new long[] { 1, 0, 1, 2, 3, 4 }, "b", 2, "post", 345, 456)]

    public void VersionIsCorrectlyParsedIntoParts(
      string version,
      long? epoch,
      string release,
      long[] releaseParts,
      string preReleaseLabel,
      long? preReleaseValue,
      string postReleaseLabel,
      long? postReleaseValue,
      long? developmentReleaseValue
    ) {
      var info = new PythonVersionInfo {Version = version};
      Assert.Equal(epoch, info.Epoch);
      Assert.Equal(release, info.Release);
      Assert.Equal(releaseParts, info.ReleaseParts);
      Assert.Equal(preReleaseLabel, info.PreReleaseLabel);
      Assert.Equal(preReleaseValue, info.PreReleaseValue);
      Assert.Equal(postReleaseLabel, info.PostReleaseLabel);
      Assert.Equal(postReleaseValue, info.PostReleaseValue);
      Assert.Equal(developmentReleaseValue, info.DevelopmentReleaseValue);
    }

    [Fact]
    public void ParseVersionThrowsExceptionIfVersionIsIncorrectlyFormatted() {
      Assert.Throws<VersionParseException>(testCode: () =>
        new PythonVersionInfo {Version = "1.0.invalid.format"});
    }

    [Theory]
    [InlineData("1!1.0", "1!1.0", 0)]
    [InlineData("10!1.0", "10!1.0", 0)]
    [InlineData("1.0", "1!1.0", -1)]
    [InlineData("1!1.0", "1.0", 1)]
    [InlineData("1.0.0", "1.0.0", 0)]
    [InlineData("1.0", "1.0.0", 0)]
    [InlineData("1.0.0", "1.0", 0)]
    [InlineData("1.0.0.0.0.0", "1.0", 0)]
    [InlineData("1.0", "1.0.0.0.0.0", 0)]
    [InlineData("1.0.0.0.0.0.1", "1.0", 1)]
    [InlineData("1.0", "1.0.0.0.0.0.1", -1)]
    [InlineData("1.0.0.1", "1.0.0", 1)]
    [InlineData("1.0.0", "1.0.0.1", -1)]
    [InlineData("2.10.0", "1.10.0", 1)]
    [InlineData("1.10.0", "2.10.0", -1)]
    [InlineData("10.0.0", "1.0.0", 1)]
    [InlineData("1.0.0", "10.0.0", -1)]
    [InlineData("2.11.0", "2.1.0", 1)]
    [InlineData("2.1.0", "2.11.0", -1)]
    [InlineData("1.18.5", "1.9.0", 1)]
    [InlineData("1.9.5", "1.18.5", -1)]
    [InlineData("12.0.0", "8.0.0", 1)]
    [InlineData("8.0.0", "12.0.0", -1)]
    [InlineData("1", "1", 0)]
    [InlineData("1.1", "1.1", 0)]
    [InlineData("1.1", "1", 1)]
    [InlineData("1", "1.1", -1)]
    [InlineData("1.1.1", "1.1", 1)]
    [InlineData("1.1", "1.1.1", -1)]
    [InlineData("2", "2.0.0", 0)]
    [InlineData("2.0", "2.0.0", 0)]
    [InlineData("2.0.0", "2", 0)]
    [InlineData("2.0.0", "2.0", 0)]
    [InlineData("2.2", "2.2.0", 0)]
    [InlineData("2.2.0", "2.2", 0)]

    public void CompareToCorrectlySortsByVersion(
      string leftVersion,
      string rightVersion,
      int expected
    ) {
      var left = new PythonVersionInfo {Version = leftVersion};
      var right = new PythonVersionInfo {Version = rightVersion};
      Assert.Equal(expected, left.CompareTo(right));
    }

    [Fact]
    public void CompareToThrowsExceptionIfOtherVersionIsNull() {
      var version = new PythonVersionInfo {Version = "1.0"};
      Assert.Throws<ArgumentException>(testCode: () => version.CompareTo(null));
    }

    [Fact]
    public void CompareToThrowsExceptionIfOtherVersionIsNotPythonVersionInfo()
    {
      var versionInfo = new PythonVersionInfo {Version = "1.0"};
      var otherVersion = new SemVerVersionInfo {Version = "1.0"};;
      Assert.Throws<ArgumentException>(testCode: () =>
        versionInfo.CompareTo(otherVersion));
    }
  }
}
