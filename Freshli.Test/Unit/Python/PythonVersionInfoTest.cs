using System;
using Freshli.Exceptions;
using Freshli.Languages.Python;
using RestSharp.Extensions;
using Xunit;

namespace Freshli.Test.Unit.Python {
  public class PythonVersionInfoTest {

    [Theory]
    [InlineData(
      "1!1",
      1,
      "1", new long[] { 1 },
      null, null, false,
      null, null, false,
      null, false)
    ]
    [InlineData(
        "10!1",
        10,
        "1", new long[] { 1 },
        null, null, false,
        null, null, false,
        null, false)
    ]
    [InlineData(
      "10!1.0",
      10,
      "1.0", new long[] { 1, 0 },
      null, null, false,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "20200101!1",
      20200101,
      "1", new long[] { 1 },
      null, null, false,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "0.1",
      0,
      "0.1", new long[] { 0, 1 },
      null, null, false,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "0.10",
      0,
      "0.10", new long[] { 0, 10 },
      null, null, false,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "0.10.1",
      0,
      "0.10.1", new long[] { 0, 10, 1 },
      null, null, false,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "1.0",
      0,
      "1.0", new long[] { 1, 0 },
      null, null, false,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "10.0",
      0,
      "10.0", new long[] { 10, 0 },
      null, null, false,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "10.0.1.0",
      0,
      "10.0.1.0", new long[] { 10, 0, 1, 0 },
      null, null, false,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "1.0.dev456",
      0,
      "1.0", new long[] { 1, 0 },
      null, null, false,
      null, null, false,
      456, true)
    ]
    [InlineData(
      "1.0a1",
      0,
      "1.0", new long[] { 1, 0 },
      "a", 1, true,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "1.0A1",
      0,
      "1.0", new long[] { 1, 0 },
      "a", 1, true,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "1.0.1a1",
      0,
      "1.0.1", new long[] { 1, 0, 1 },
      "a", 1, true,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "1.0a2.dev456",
      0,
      "1.0", new long[] { 1, 0 },
      "a", 2, true,
      null, null, false,
      456, true)
    ]
    [InlineData(
      "1.0a12.dev456",
      0,
      "1.0", new long[] { 1, 0 },
      "a", 12, true,
      null, null, false,
      456, true)
    ]
    [InlineData(
      "1.0a12.post456",
      0,
      "1.0", new long[] { 1, 0 },
      "a", 12, true,
      "post", 456, true,
      null, false)
    ]
    [InlineData(
      "1.0a12",
      0,
      "1.0", new long[] { 1, 0 },
      "a", 12, true,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "1.0b1.dev456",
      0,
      "1.0", new long[] { 1, 0 },
      "b", 1, true,
      null, null, false,
      456, true)
    ]
    [InlineData(
      "1.0b2",
      0,
      "1.0", new long[] { 1, 0 },
      "b", 2, true,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "1.0b2.post345.dev456",
      0,
      "1.0", new long[] { 1, 0 },
      "b", 2, true,
      "post", 345, true,
      456, true)
    ]
    [InlineData(
      "1.0b2.post345",
      0,
      "1.0", new long[] { 1, 0 },
      "b", 2, true,
      "post", 345, true,
      null, false)
    ]
    [InlineData(
      "1.0rc1",
      0,
      "1.0", new long[] { 1, 0 },
      "rc", 1, true,
      null, null, false,
      null, false)
    ]
    [InlineData(
      "1.0rc1.dev456",
      0,
      "1.0", new long[] { 1, 0 },
      "rc", 1, true,
      null, null, false,
      456, true)
    ]
    [InlineData(
      "1.0rc1.post456",
      0,
      "1.0", new long[] { 1, 0 },
      "rc", 1, true,
      "post", 456, true,
      null, false)
    ]
    [InlineData(
      "1.0.post456.dev34",
      0,
      "1.0", new long[] { 1, 0 },
      null, null, false,
      "post", 456, true,
      34, true)
    ]
    [InlineData(
      "1.0.post456",
      0,
      "1.0", new long[] { 1, 0 },
      null, null, false,
      "post", 456, true,
      null, false)
    ]
    [InlineData(
      "1.0.POST456",
      0,
      "1.0", new long[] { 1, 0 },
      null, null, false,
      "post", 456, true,
      null, false)
    ]
    [InlineData(
      "1.1.dev1",
      0,
      "1.1", new long[] { 1, 1 },
      null, null, false,
      null, null, false,
      1, true)
    ]
    [InlineData(
      "1.1.DEV1",
      0,
      "1.1", new long[] { 1, 1 },
      null, null, false,
      null, null, false,
      1, true)
    ]
    [InlineData(
      "2020!1.0.1.2.3.4b2.dev456",
      2020,
      "1.0.1.2.3.4", new long[] { 1, 0, 1, 2, 3, 4 },
      "b", 2, true,
      null, null, false,
      456, true)
    ]
    [InlineData(
      "2020!1.0.1.2.3.4.post345.dev456",
      2020,
      "1.0.1.2.3.4", new long[] { 1, 0, 1, 2, 3, 4 },
      null, null, false,
      "post", 345, true,
      456, true)
    ]


    public void VersionIsCorrectlyParsedIntoParts(
      string version,
      long? epoch,
      string release,
      long[] releaseParts,
      string preReleaseLabel,
      long? preReleaseIncrement,
      bool isPreRelease,
      string postReleaseLabel,
      long? postReleaseIncrement,
      bool isPostRelease,
      long? developmentReleaseIncrement,
      bool isDevelopmentRelease
    ) {
      var info = new PythonVersionInfo {Version = version};
      Assert.Equal(epoch, info.Epoch);
      Assert.Equal(release, info.Release);
      Assert.Equal(releaseParts, info.ReleaseParts);

      if (preReleaseLabel != null) {
        Assert.Equal(preReleaseLabel, info.PreRelease.Label);
      } else if (info.PreRelease != null) {
        Assert.Null(info.PreRelease.Label);
      } else {
        Assert.Null(info.PreRelease);
      }

      if (preReleaseIncrement != null) {
        Assert.Equal(preReleaseIncrement, info.PreRelease.Increment);
      } else if (info.PreRelease != null) {
        Assert.Null(info.PreRelease.Increment);
      } else {
        Assert.Null(info.PreRelease);
      }

      Assert.Equal(isPreRelease, info.IsPreRelease);

      if (postReleaseLabel != null) {
        Assert.Equal(postReleaseLabel, info.PostRelease.Label);
      } else if (info.PostRelease != null) {
        Assert.Null(info.PostRelease.Label);
      } else {
        Assert.Null(info.PostRelease);
      }

      if (postReleaseIncrement != null) {
        Assert.Equal(postReleaseIncrement, info.PostRelease.Increment);
      } else if (info.PostRelease != null) {
        Assert.Null(info.PostRelease.Increment);
      } else {
        Assert.Null(info.PostRelease);
      }
      Assert.Equal(isPostRelease, info.IsPostRelease);

      if (developmentReleaseIncrement != null) {
        Assert.Equal(
          developmentReleaseIncrement,
          info.DevelopmentRelease.Increment
        );
      } else if (info.DevelopmentRelease != null) {
        Assert.Null(info.DevelopmentRelease.Increment);
      } else {
        Assert.Null(info.DevelopmentRelease);
      }

      Assert.Equal(isDevelopmentRelease, info.IsDevelopmentRelease);
    }

    [Fact]
    public void ParseVersionThrowsExceptionIfVersionIsIncorrectlyFormatted() {
      Assert.Throws<VersionParseException>(testCode: () =>
        new PythonVersionInfo {Version = "1.0.invalid.format"});
    }

    [Theory]
    [InlineData(
      "1.0.dev456",
      PythonVersionInfo.SuffixType.Development,
      null,
      null)
    ]
    [InlineData(
      "1.0a1",
      PythonVersionInfo.SuffixType.Pre,
      PythonVersionInfo.SuffixType.NoSuffix,
      null)
    ]
    [InlineData(
      "1.0a2.dev456",
      PythonVersionInfo.SuffixType.Pre,
      PythonVersionInfo.SuffixType.Development,
      null)
    ]
    [InlineData(
      "1.0a12.dev456",
      PythonVersionInfo.SuffixType.Pre,
      PythonVersionInfo.SuffixType.Development,
      null)
    ]
    [InlineData(
      "1.0a12",
      PythonVersionInfo.SuffixType.Pre,
      PythonVersionInfo.SuffixType.NoSuffix,
      null)
    ]
    [InlineData(
      "1.0b1.dev456",
      PythonVersionInfo.SuffixType.Pre,
      PythonVersionInfo.SuffixType.Development,
      null)
    ]
    [InlineData(
      "1.0b2",
      PythonVersionInfo.SuffixType.Pre,
      PythonVersionInfo.SuffixType.NoSuffix,
      null)
    ]
    [InlineData(
      "1.0b2.post345.dev456",
      PythonVersionInfo.SuffixType.Pre,
      PythonVersionInfo.SuffixType.Post,
      PythonVersionInfo.SuffixType.Development)
    ]
    [InlineData(
      "1.0b2.post345",
      PythonVersionInfo.SuffixType.Pre,
      PythonVersionInfo.SuffixType.Post,
      PythonVersionInfo.SuffixType.NoSuffix)
    ]
    [InlineData(
      "1.0rc1.dev456",
      PythonVersionInfo.SuffixType.Pre,
      PythonVersionInfo.SuffixType.Development,
      null)
    ]
    [InlineData(
      "1.0rc1",
      PythonVersionInfo.SuffixType.Pre,
      PythonVersionInfo.SuffixType.NoSuffix,
      null)
    ]
    [InlineData(
      "1.0",
      PythonVersionInfo.SuffixType.NoSuffix,
      null,
      null)
    ]
    [InlineData(
      "1.0.post456.dev34",
      PythonVersionInfo.SuffixType.Post,
      null,
      PythonVersionInfo.SuffixType.Development)
    ]
    [InlineData(
      "1.0.post456",
      PythonVersionInfo.SuffixType.Post,
      null,
      PythonVersionInfo.SuffixType.NoSuffix)
    ]

    public void SortPositionsAreCorrectlySet(
      string version,
      int? releaseSortPosition,
      int? preReleaseSortPosition,
      int? postReleaseSortPosition
    ) {
      var versionInfo = new PythonVersionInfo {Version = version};
      Assert.Equal(releaseSortPosition, versionInfo.ReleaseSuffixType);
      Assert.Equal(preReleaseSortPosition, versionInfo.PreReleaseSuffixType);
      Assert.Equal(postReleaseSortPosition, versionInfo.PostReleaseSuffixType);
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
    [InlineData("1.0.dev456", "1.0.dev678", -1)]
    [InlineData("1.0.dev456", "1.0a1", -1)]
    [InlineData("1.0.dev456", "1.0a2.dev456", -1)]
    [InlineData("1.0.dev456", "1.0a12.dev456", -1)]
    [InlineData("1.0.dev456", "1.0a12", -1)]
    [InlineData("1.0.dev456", "1.0b1.dev456", -1)]
    [InlineData("1.0.dev456", "1.0b2", -1)]
    [InlineData("1.0.dev456", "1.0b2.post345.dev456", -1)]
    [InlineData("1.0.dev456", "1.0b2.post345", -1)]
    [InlineData("1.0.dev456", "1.0rc1.dev456", -1)]
    [InlineData("1.0.dev456", "1.0rc1", -1)]
    [InlineData("1.0.dev456", "1.0", -1)]
    [InlineData("1.0.dev456", "1.0.post456.dev34", -1)]
    [InlineData("1.0.dev456", "1.0.post456", -1)]
    [InlineData("1.0.dev456", "1.1.dev1", -1)]
    [InlineData("1.0a1", "1.0a2.dev456", -1)]
    [InlineData("1.0a1", "1.0a12.dev456", -1)]
    [InlineData("1.0a1", "1.0a12", -1)]
    [InlineData("1.0a1", "1.0b1.dev456", -1)]
    [InlineData("1.0a1", "1.0b2", -1)]
    [InlineData("1.0a1", "1.0b2.post345.dev456", -1)]
    [InlineData("1.0a1", "1.0b2.post345", -1)]
    [InlineData("1.0a1", "1.0rc1.dev456", -1)]
    [InlineData("1.0a1", "1.0rc1", -1)]
    [InlineData("1.0a1", "1.0", -1)]
    [InlineData("1.0a1", "1.0.post456.dev34", -1)]
    [InlineData("1.0a1", "1.0.post456", -1)]
    [InlineData("1.0a1", "1.1.dev1", -1)]
    [InlineData("1.0a2.dev456", "1.0a12.dev456", -1)]
    [InlineData("1.0a2.dev456", "1.0a12", -1)]
    [InlineData("1.0a2.dev456", "1.0b1.dev456", -1)]
    [InlineData("1.0a2.dev456", "1.0b2", -1)]
    [InlineData("1.0a2.dev456", "1.0b2.post345.dev456", -1)]
    [InlineData("1.0a2.dev456", "1.0b2.post345", -1)]
    [InlineData("1.0a2.dev456", "1.0rc1.dev456", -1)]
    [InlineData("1.0a2.dev456", "1.0rc1", -1)]
    [InlineData("1.0a2.dev456", "1.0", -1)]
    [InlineData("1.0a2.dev456", "1.0.post456.dev34", -1)]
    [InlineData("1.0a2.dev456", "1.0.post456", -1)]
    [InlineData("1.0a2.dev456", "1.1.dev1", -1)]
    [InlineData("1.0a12.dev456", "1.0a12", -1)]
    [InlineData("1.0a12.dev456", "1.0b1.dev456", -1)]
    [InlineData("1.0a12.dev456", "1.0b2", -1)]
    [InlineData("1.0a12.dev456", "1.0b2.post345.dev456", -1)]
    [InlineData("1.0a12.dev456", "1.0b2.post345", -1)]
    [InlineData("1.0a12.dev456", "1.0rc1.dev456", -1)]
    [InlineData("1.0a12.dev456", "1.0rc1", -1)]
    [InlineData("1.0a12.dev456", "1.0", -1)]
    [InlineData("1.0a12.dev456", "1.0.post456.dev34", -1)]
    [InlineData("1.0a12.dev456", "1.0.post456", -1)]
    [InlineData("1.0a12.dev456", "1.1.dev1", -1)]
    [InlineData("1.0a12", "1.0b1.dev456", -1)]
    [InlineData("1.0a12", "1.0b2", -1)]
    [InlineData("1.0a12", "1.0b2.post345.dev456", -1)]
    [InlineData("1.0a12", "1.0b2.post345", -1)]
    [InlineData("1.0a12", "1.0rc1.dev456", -1)]
    [InlineData("1.0a12", "1.0rc1", -1)]
    [InlineData("1.0a12", "1.0", -1)]
    [InlineData("1.0a12", "1.0.post456.dev34", -1)]
    [InlineData("1.0a12", "1.0.post456", -1)]
    [InlineData("1.0a12", "1.1.dev1", -1)]
    [InlineData("1.0b1.dev456", "1.0b2", -1)]
    [InlineData("1.0b1.dev456", "1.0b2.post345.dev456", -1)]
    [InlineData("1.0b1.dev456", "1.0b2.post345", -1)]
    [InlineData("1.0b1.dev456", "1.0rc1.dev456", -1)]
    [InlineData("1.0b1.dev456", "1.0rc1", -1)]
    [InlineData("1.0b1.dev456", "1.0", -1)]
    [InlineData("1.0b1.dev456", "1.0.post456.dev34", -1)]
    [InlineData("1.0b1.dev456", "1.0.post456", -1)]
    [InlineData("1.0b1.dev456", "1.1.dev1", -1)]
    [InlineData("1.0b2", "1.0b2.post345.dev456", -1)]
    [InlineData("1.0b2", "1.0b2.post345", -1)]
    [InlineData("1.0b2", "1.0rc1.dev456", -1)]
    [InlineData("1.0b2", "1.0rc1", -1)]
    [InlineData("1.0b2", "1.0", -1)]
    [InlineData("1.0b2", "1.0.post456.dev34", -1)]
    [InlineData("1.0b2", "1.0.post456", -1)]
    [InlineData("1.0b2", "1.1.dev1", -1)]
    [InlineData("1.0b2.post345.dev456", "1.0b2.post345", -1)]
    [InlineData("1.0b2.post345.dev456", "1.0rc1.dev456", -1)]
    [InlineData("1.0b2.post345.dev456", "1.0rc1", -1)]
    [InlineData("1.0b2.post345.dev456", "1.0", -1)]
    [InlineData("1.0b2.post345.dev456", "1.0.post456.dev34", -1)]
    [InlineData("1.0b2.post345.dev456", "1.0.post456", -1)]
    [InlineData("1.0b2.post345.dev456", "1.1.dev1", -1)]
    [InlineData("1.0b2.post345", "1.0rc1.dev456", -1)]
    [InlineData("1.0b2.post345", "1.0rc1", -1)]
    [InlineData("1.0b2.post345", "1.0", -1)]
    [InlineData("1.0b2.post345", "1.0.post456.dev34", -1)]
    [InlineData("1.0b2.post345", "1.0.post456", -1)]
    [InlineData("1.0b2.post345", "1.1.dev1", -1)]
    [InlineData("1.0rc1.dev456", "1.0rc1", -1)]
    [InlineData("1.0rc1.dev456", "1.0", -1)]
    [InlineData("1.0rc1.dev456", "1.0.post456.dev34", -1)]
    [InlineData("1.0rc1.dev456", "1.0.post456", -1)]
    [InlineData("1.0rc1.dev456", "1.1.dev1", -1)]
    [InlineData("1.0rc1", "1.0", -1)]
    [InlineData("1.0rc1", "1.0.post456.dev34", -1)]
    [InlineData("1.0rc1", "1.0.post456", -1)]
    [InlineData("1.0rc1", "1.1.dev1", -1)]
    [InlineData("1.0", "1.0.post456.dev34", -1)]
    [InlineData("1.0", "1.0.post456", -1)]
    [InlineData("1.0", "1.1.dev1", -1)]
    [InlineData("1.0.post456.dev34", "1.0.post456", -1)]
    [InlineData("1.0.post456.dev34", "1.1.dev1", -1)]
    [InlineData("1.0.post456", "1.1.dev1", -1)]
    [InlineData("1.0a2", "2.0a1", -1)]
    [InlineData("1.0a1", "2.0a1", -1)]
    [InlineData("1.0.post345", "2.0.post1", -1)]
    [InlineData("1.0.post345", "2.0.post345", -1)]
    [InlineData("1.0.dev345", "2.0.dev1", -1)]
    [InlineData("1.0.dev345", "2.0.dev345", -1)]
    [InlineData("1.0.dev678", "1.0.dev456", 1)]
    [InlineData("1.0a1", "1.0.dev456", 1)]
    [InlineData("1.0a2.dev456", "1.0.dev456", 1)]
    [InlineData("1.0a12.dev456", "1.0.dev456", 1)]
    [InlineData("1.0a12", "1.0.dev456", 1)]
    [InlineData("1.0b1.dev456", "1.0.dev456", 1)]
    [InlineData("1.0b2", "1.0.dev456", 1)]
    [InlineData("1.0b2.post345.dev456", "1.0.dev456", 1)]
    [InlineData("1.0b2.post345", "1.0.dev456", 1)]
    [InlineData("1.0rc1.dev456", "1.0.dev456", 1)]
    [InlineData("1.0rc1", "1.0.dev456", 1)]
    [InlineData("1.0", "1.0.dev456", 1)]
    [InlineData("1.0.post456.dev34", "1.0.dev456", 1)]
    [InlineData("1.0.post456", "1.0.dev456", 1)]
    [InlineData("1.1.dev1", "1.0.dev456", 1)]
    [InlineData("1.0a2.dev456", "1.0a1", 1)]
    [InlineData("1.0a12.dev456", "1.0a1", 1)]
    [InlineData("1.0a12", "1.0a1", 1)]
    [InlineData("1.0b1.dev456", "1.0a1", 1)]
    [InlineData("1.0b2", "1.0a1", 1)]
    [InlineData("1.0b2.post345.dev456", "1.0a1", 1)]
    [InlineData("1.0b2.post345", "1.0a1", 1)]
    [InlineData("1.0rc1.dev456", "1.0a1", 1)]
    [InlineData("1.0rc1", "1.0a1", 1)]
    [InlineData("1.0", "1.0a1", 1)]
    [InlineData("1.0.post456.dev34", "1.0a1", 1)]
    [InlineData("1.0.post456", "1.0a1", 1)]
    [InlineData("1.1.dev1", "1.0a1", 1)]
    [InlineData("1.0a12.dev456", "1.0a2.dev456", 1)]
    [InlineData("1.0a12", "1.0a2.dev456", 1)]
    [InlineData("1.0b1.dev456", "1.0a2.dev456", 1)]
    [InlineData("1.0b2", "1.0a2.dev456", 1)]
    [InlineData("1.0b2.post345.dev456", "1.0a2.dev456", 1)]
    [InlineData("1.0b2.post345", "1.0a2.dev456", 1)]
    [InlineData("1.0rc1.dev456", "1.0a2.dev456", 1)]
    [InlineData("1.0rc1","1.0a2.dev456", 1)]
    [InlineData("1.0", "1.0a2.dev456", 1)]
    [InlineData("1.0.post456.dev34", "1.0a2.dev456", 1)]
    [InlineData("1.0.post456", "1.0a2.dev456", 1)]
    [InlineData("1.1.dev1", "1.0a2.dev456", 1)]
    [InlineData("1.0a12", "1.0a12.dev456", 1)]
    [InlineData("1.0b1.dev456", "1.0a12.dev456", 1)]
    [InlineData("1.0b2", "1.0a12.dev456", 1)]
    [InlineData("1.0b2.post345.dev456", "1.0a12.dev456", 1)]
    [InlineData("1.0b2.post345", "1.0a12.dev456", 1)]
    [InlineData("1.0rc1.dev456", "1.0a12.dev456", 1)]
    [InlineData("1.0rc1", "1.0a12.dev456", 1)]
    [InlineData("1.0", "1.0a12.dev456", 1)]
    [InlineData("1.0.post456.dev34", "1.0a12.dev456", 1)]
    [InlineData("1.0.post456", "1.0a12.dev456", 1)]
    [InlineData("1.1.dev1","1.0a12.dev456", 1)]
    [InlineData("1.0b1.dev456", "1.0a12", 1)]
    [InlineData("1.0b2", "1.0a12", 1)]
    [InlineData("1.0b2.post345.dev456", "1.0a12", 1)]
    [InlineData("1.0b2.post345", "1.0a12", 1)]
    [InlineData("1.0rc1.dev456", "1.0a12", 1)]
    [InlineData("1.0rc1", "1.0a12", 1)]
    [InlineData("1.0", "1.0a12", 1)]
    [InlineData("1.0.post456.dev34", "1.0a12", 1)]
    [InlineData("1.0.post456", "1.0a12", 1)]
    [InlineData("1.1.dev1", "1.0a12", 1)]
    [InlineData("1.0b2", "1.0b1.dev456", 1)]
    [InlineData("1.0b2.post345.dev456", "1.0b1.dev456", 1)]
    [InlineData("1.0b2.post345", "1.0b1.dev456", 1)]
    [InlineData("1.0rc1.dev456", "1.0b1.dev456", 1)]
    [InlineData("1.0rc1", "1.0b1.dev456", 1)]
    [InlineData("1.0", "1.0b1.dev456", 1)]
    [InlineData("1.0.post456.dev34", "1.0b1.dev456", 1)]
    [InlineData("1.0.post456", "1.0b1.dev456", 1)]
    [InlineData("1.1.dev1", "1.0b1.dev456", 1)]
    [InlineData("1.0b2.post345.dev456", "1.0b2", 1)]
    [InlineData("1.0b2.post345", "1.0b2", 1)]
    [InlineData("1.0rc1.dev456", "1.0b2", 1)]
    [InlineData("1.0rc1", "1.0b2", 1)]
    [InlineData("1.0", "1.0b2", 1)]
    [InlineData("1.0.post456.dev34", "1.0b2", 1)]
    [InlineData("1.0.post456", "1.0b2", 1)]
    [InlineData("1.1.dev1", "1.0b2", 1)]
    [InlineData("1.0b2.post345", "1.0b2.post345.dev456", 1)]
    [InlineData("1.0rc1.dev456", "1.0b2.post345.dev456", 1)]
    [InlineData("1.0rc1", "1.0b2.post345.dev456", 1)]
    [InlineData("1.0", "1.0b2.post345.dev456", 1)]
    [InlineData("1.0.post456.dev34","1.0b2.post345.dev456", 1)]
    [InlineData("1.0.post456", "1.0b2.post345.dev456", 1)]
    [InlineData("1.1.dev1", "1.0b2.post345.dev456", 1)]
    [InlineData("1.0rc1.dev456", "1.0b2.post345", 1)]
    [InlineData("1.0rc1", "1.0b2.post345", 1)]
    [InlineData("1.0", "1.0b2.post345", 1)]
    [InlineData("1.0.post456.dev34", "1.0b2.post345", 1)]
    [InlineData("1.0.post456", "1.0b2.post345", 1)]
    [InlineData("1.1.dev1", "1.0b2.post345", 1)]
    [InlineData("1.0rc1", "1.0rc1.dev456", 1)]
    [InlineData("1.0", "1.0rc1.dev456", 1)]
    [InlineData("1.0.post456.dev34", "1.0rc1.dev456", 1)]
    [InlineData("1.0.post456", "1.0rc1.dev456", 1)]
    [InlineData("1.1.dev1", "1.0rc1.dev456", 1)]
    [InlineData("1.0", "1.0rc1", 1)]
    [InlineData("1.0.post456.dev34", "1.0rc1", 1)]
    [InlineData("1.0.post456", "1.0rc1", 1)]
    [InlineData("1.1.dev1", "1.0rc1", 1)]
    [InlineData("1.0.post456.dev34", "1.0", 1)]
    [InlineData("1.0.post456", "1.0", 1)]
    [InlineData("1.1.dev1", "1.0", 1)]
    [InlineData("1.0.post456", "1.0.post456.dev34", 1)]
    [InlineData("1.1.dev1", "1.0.post456.dev34", 1)]
    [InlineData("1.1.dev1", "1.0.post456", 1)]
    [InlineData("2.0a1", "1.0a2", 1)]
    [InlineData("2.0a1", "1.0a1", 1)]
    [InlineData("2.0.post1", "1.0.post345", 1)]
    [InlineData("2.0.post345", "1.0.post345", 1)]
    [InlineData("2.0.dev1", "1.0.dev345", 1)]
    [InlineData("2.0.dev345", "1.0.dev345", 1)]

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
      Assert.Throws<ArgumentException>(testCode: () =>
        version.CompareTo(null));
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
