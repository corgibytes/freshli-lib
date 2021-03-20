using System;
using Corgibytes.Freshli.Lib.Languages.Ruby;
using Xunit;
using Xunit.Abstractions;

namespace Corgibytes.Freshli.Lib.Test.Unit.Ruby {
  public class RubyGemsVersionInfoTest {
    private readonly ITestOutputHelper _testOutputHelper;

    public RubyGemsVersionInfoTest(ITestOutputHelper testOutputHelper) {
      _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("0.9",  new[] { "0", "9" }, false, false, null)]
    [InlineData("1", new[] { "1" }, false, false, null)]
    [InlineData("1-x86-linux", new[] { "1" }, false, true, "x86-linux")]
    [InlineData("1.0.a.2", new[] { "1", "0", "a", "2" }, true, false, null)]
    [InlineData("1.0.b1", new[] { "1", "0", "b", "1" }, true, false, null)]
    [InlineData("1.0.b10", new[] { "1", "0", "b", "10" }, true, false, null)]
    [InlineData("1.0009",  new[] { "1", "0009" }, false, false, null)]
    [InlineData("1.0045",  new[] { "1", "0045" }, false, false, null)]
    [InlineData("1.2", new[] { "1", "2" }, false, false, null)]
    [InlineData("1.2-x86-linux", new[] { "1", "2" }, false, true, "x86-linux")]
    [InlineData("1.2.3", new[] { "1", "2", "3" }, false, false, null)]
    [InlineData("1.2.3-x86-linux",
      new[] { "1", "2", "3" }, false, true, "x86-linux")]
    [InlineData("1.2.3.4", new[] { "1", "2", "3", "4" }, false, false, null)]
    [InlineData("1.2.3.4-x86-linux",
      new[] { "1", "2", "3", "4" }, false, true, "x86-linux")]
    [InlineData("1.2.3.4.5",
      new[] { "1", "2", "3", "4", "5" }, false, false, null)]
    [InlineData("1.2.3.4.5-x86-linux",
      new[] { "1", "2", "3", "4", "5" }, false, true, "x86-linux")]
    [InlineData("1.2.3.4.5.6",
      new[] { "1", "2", "3", "4", "5", "6" }, false, false, null)]
    [InlineData("1.2.3.4.5.6-x86-linux",
      new[] { "1", "2", "3", "4", "5", "6" }, false, true, "x86-linux")]
    [InlineData("1.2.3a1",
      new[] { "1", "2", "3", "a", "1" }, true, false, null)]
    [InlineData("1.5.6.123alpha",
      new[] { "1", "5", "6", "123", "alpha" }, true, false, null)]
    [InlineData("1.5.6.123alpha123",
      new[] { "1", "5", "6", "123", "alpha", "123" }, true, false, null)]
    [InlineData("1.5.6.1alpha",
      new[] { "1", "5", "6", "1", "alpha" }, true, false, null)]
    [InlineData("1.5.6.rc1",
      new[] { "1", "5", "6", "rc", "1" }, true, false, null)]
    [InlineData("1.5.6.rc1-x86-linux",
      new[] { "1", "5", "6", "rc", "1" }, true, true, "x86-linux")]
    [InlineData("1.5.6.rc2",
      new[] { "1", "5", "6", "rc", "2" }, true, false, null)]
    [InlineData("1.5.6.rc22",
      new[] { "1", "5", "6", "rc", "22" }, true, false, null)]
    [InlineData("1.5.6.rc22alpha",
      new[] { "1", "5", "6", "rc", "22", "alpha" }, true, false, null)]
    [InlineData("1.5.6.rc22alpha1",
      new[] { "1", "5", "6", "rc", "22", "alpha", "1" }, true, false, null)]
    [InlineData("2.0.20110131120940",
      new[] { "2", "0", "20110131120940" }, false, false, null)]
    [InlineData("2.0.8.beta.20110131120940",
      new[] { "2", "0", "8", "beta", "20110131120940" }, true, false, null)]
    [InlineData("2.20110131120940",
      new[] { "2", "20110131120940" }, false, false, null)]
    [InlineData("20110131120940",
      new[] { "20110131120940" }, false, false, null)]
    [InlineData("3.6.0", new[] { "3", "6", "0" }, false, false, null)]
    [InlineData("3.6.0-x86-linux",
      new[] { "3", "6", "0" }, false, true, "x86-linux")]
    public void VersionIsParsedIntoParts(
      string version,
      string[] versionParts,
      bool isPreRelease,
      bool isPlatformSpecific,
      string platformSpecifier
    ) {
      var info = new RubyGemsVersionInfo() {Version = version};

      var count = info.VersionParts.Count;

      _testOutputHelper.WriteLine($"Count = {count}");

      Assert.Equal(versionParts, info.VersionParts);
      Assert.Equal(isPreRelease, info.IsPreRelease);
      Assert.Equal(isPlatformSpecific, info.IsPlatformSpecific);
      Assert.Equal(platformSpecifier, info.PlatformSpecifier);
    }

    [Theory]
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
    [InlineData("1.0.0", "1.0.0a1", 1)]
    [InlineData("1.0.0a1", "1.0.0", -1)]
    [InlineData("1.0.0a12", "1.0.0a8", 1)]
    [InlineData("1.0.0a8", "1.0.0a12", -1)]
    [InlineData("1.0.0a.12", "1.0.0a.8", 1)]
    [InlineData("1.0.0a.8", "1.0.0a.12", -1)]
    [InlineData("1.0.0.beta", "1.0.0.alpha", 1)]
    [InlineData("1.0.0.alpha", "1.0.0.beta", -1)]
    [InlineData("1.0.0.alpha1", "1.0.0.alpha", 1)]
    [InlineData("1.0.0.alpha", "1.0.0.alpha1", -1)]
    [InlineData("1.0.0", "1.0.0.alpha", 1)]
    [InlineData("1.0.0.alpha", "1.0.0", -1)]
    [InlineData("1", "1", 0)]
    [InlineData("1.1", "1.1", 0)]
    [InlineData("1.1.1.alpha", "1.1.1.alpha", 0)]
    [InlineData("1.1", "1", 1)]
    [InlineData("1", "1.1", -1)]
    [InlineData("1.1.1", "1.1", 1)]
    [InlineData("1.1", "1.1.1", -1)]
    [InlineData("1.1.1", "1.1.1.test", 1)]
    [InlineData("1.1.1.test", "1.1.1", -1)]
    [InlineData("1.1.1.rc2", "1.1.1.rc2", 0)]
    [InlineData("2", "2.0.0", 0)]
    [InlineData("2.0", "2.0.0", 0)]
    [InlineData("2.0.0", "2", 0)]
    [InlineData("2.0.0", "2.0", 0)]
    [InlineData("2.2", "2.2.0", 0)]
    [InlineData("2.2.0", "2.2", 0)]
    public void CompareToSortsByVersion(
      string leftVersion,
      string rightVersion,
      int expected
    ) {
      var left = new RubyGemsVersionInfo {Version = leftVersion};
      var right = new RubyGemsVersionInfo {Version = rightVersion};
      Assert.Equal(expected, left.CompareTo(right));
    }

    [Fact]
    public void CompareToThrowsExceptionIfOtherVersionIsNull() {
      var version = new RubyGemsVersionInfo {Version = "1.0"};
      Assert.Throws<ArgumentException>(() => version.CompareTo(null));
    }

    [Fact]
    public void CompareToThrowsExceptionIfOtherVersionIsNotRubyGemsVersionInfo()
    {
      var versionInfo = new RubyGemsVersionInfo {Version = "1.0"};
      var otherVersion = new SemVerVersionInfo("1.0");
      Assert.Throws<ArgumentException>(() =>
        versionInfo.CompareTo(otherVersion));
    }
  }
}
