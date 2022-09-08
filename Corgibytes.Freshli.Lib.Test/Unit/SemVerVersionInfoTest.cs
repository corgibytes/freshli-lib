using System;
using Corgibytes.Freshli.Lib.Exceptions;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit
{
    public class SemVerVersionInfoTest
    {
        [Theory]
        [InlineData("1", 1L, null, null, false, null, null)]
        [InlineData("v3.6.0", 3L, 6L, 0L, false, null, null)]
        [InlineData("1.2", 1L, 2L, null, false, null, null)]
        [InlineData("1.2.3", 1L, 2L, 3L, false, null, null)]
        [InlineData("1.2.3a1", 1L, 2L, 3L, true, "a1", null)]
        [InlineData("1.2.3-a1", 1L, 2L, 3L, true, "a1", null)]
        [InlineData("1.2.3a1+dev", 1L, 2L, 3L, true, "a1", "dev")]
        [InlineData("1.2.3-a1+dev", 1L, 2L, 3L, true, "a1", "dev")]
        [InlineData("1.0045", 1L, 0045L, null, false, null, null)]
        [InlineData("1.0009", 1L, 0009L, null, false, null, null)]
        [InlineData("1.301001_050", 1L, 301001L, 050L, false, null, null)]
        [InlineData("20110131120940", 20110131120940L, null, null, false, null,
          null)]
        [InlineData("2.20110131120940", 2L, 20110131120940L, null, false, null, null)]
        [InlineData("2.0.20110131120940", 2L, 0L, 20110131120940L, false, null, null)]
        [InlineData("2.0.8.beta.20110131120940", 2L, 0L, 8L, true,
          "beta.20110131120940", null)]
        [InlineData("1.0.0-alpha", 1L, 0L, 0L, true, "alpha", null)]
        [InlineData("1.0.0-alpha.1", 1L, 0L, 0L, true, "alpha.1", null)]
        [InlineData("1.0.0-0.3.7", 1L, 0L, 0L, true, "0.3.7", null)]
        [InlineData("1.0.0-x.7.z.92", 1L, 0L, 0L, true, "x.7.z.92", null)]
        [InlineData("1.0.0-x-y-z.-", 1L, 0L, 0L, true, "x-y-z.-", null)]
        [InlineData("1.0.0-alpha+001", 1L, 0L, 0L, true, "alpha", "001")]
        [InlineData("1.0.0+20130313144700", 1L, 0L, 0L, false, null, "20130313144700")]
        [InlineData("1.0.0-beta+exp.sha.5114f85", 1L, 0L, 0L, true, "beta",
          "exp.sha.5114f85")]
        [InlineData("1.0.0+21AF26D3--117B344092BD", 1L, 0L, 0L, false, null,
          "21AF26D3--117B344092BD")]
        public void VersionIsParsedIntoParts(
          string version,
          long? major,
          long? minor,
          long? patch,
          bool isPreRelease,
          string preRelease,
          string buildMetadata
        )
        {
            var info = new SemVerVersionInfo(version);
            Assert.Equal(major, info.Major);
            Assert.Equal(minor, info.Minor);
            Assert.Equal(patch, info.Patch);
            Assert.Equal(isPreRelease, info.IsPreRelease);
            Assert.Equal(preRelease, info.PreRelease);
            Assert.Equal(buildMetadata, info.BuildMetadata);
        }

        [Theory]
        [InlineData("1.0.0-alpha", "alpha", null)]
        [InlineData("1.0.0-alpha12", "alpha", 12)]
        [InlineData("1.0.0-alpha.12", "alpha", 12)]
        public void PreReleaseIsParsedIntoParts(
          string version,
          string preReleaseLabel,
          int? preReleaseIncrement
        )
        {
            var info = new SemVerVersionInfo(version);
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
        public void CompareToSortsByVersionFirst(
          string leftVersion,
          string rightVersion,
          int expected
        )
        {
            var left = new SemVerVersionInfo(leftVersion);
            var right = new SemVerVersionInfo(rightVersion);
            Assert.Equal(expected, left.CompareTo(right));
        }

        [Theory]
        [InlineData("dev-master")]
        public void ParseVersionThrowsErrorIfNotParsable(string version)
        {
            Assert.Throws<VersionParseException>
              (() => new SemVerVersionInfo(version));
        }

        [Fact]
        public void CompareToThrowsExceptionIfNull()
        {
            Assert.Throws<ArgumentException>
              (() => new SemVerVersionInfo("1.0.0").CompareTo(null));
        }

        [Fact]
        public void ConvertsToString()
        {
            var versionInfo = new SemVerVersionInfo("1.0.0");
            Assert.Equal(
              $"{nameof(versionInfo.Major)}: {versionInfo.Major}, " +
              $"{nameof(versionInfo.Minor)}: {versionInfo.Minor}, " +
              $"{nameof(versionInfo.Patch)}: {versionInfo.Patch}, " +
              $"{nameof(versionInfo.PreRelease)}: {versionInfo.PreRelease}, " +
              $"{nameof(versionInfo.BuildMetadata)}: {versionInfo.BuildMetadata}, " +
              $"{nameof(versionInfo.DatePublished)}: {versionInfo.DatePublished:O}",
              versionInfo.ToString()
            );
        }
    }
}
