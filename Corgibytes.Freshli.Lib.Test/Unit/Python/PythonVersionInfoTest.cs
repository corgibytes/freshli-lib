using System;
using Corgibytes.Freshli.Lib.Exceptions;
using Corgibytes.Freshli.Lib.Languages.Python;
using RestSharp.Extensions;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit.Python
{
    public class PythonVersionInfoTest
    {

        [Theory]
        [InlineData(
          "1!1",
          1L,
          "1", new long[] { 1 },
          null, null, false,
          null, null, false,
          null, false)
        ]
        [InlineData(
            "10!1",
            10L,
            "1", new long[] { 1 },
            null, null, false,
            null, null, false,
            null, false)
        ]
        [InlineData(
          "10!1.0",
          10L,
          "1.0", new long[] { 1, 0 },
          null, null, false,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "20200101!1",
          20200101L,
          "1", new long[] { 1 },
          null, null, false,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "0.1",
          0L,
          "0.1", new long[] { 0, 1 },
          null, null, false,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "0.10",
          0L,
          "0.10", new long[] { 0, 10 },
          null, null, false,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "0.10.1",
          0L,
          "0.10.1", new long[] { 0, 10, 1 },
          null, null, false,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "1.0",
          0L,
          "1.0", new long[] { 1, 0 },
          null, null, false,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "10.0",
          0L,
          "10.0", new long[] { 10, 0 },
          null, null, false,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "10.0.1.0",
          0L,
          "10.0.1.0", new long[] { 10, 0, 1, 0 },
          null, null, false,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "1.0.dev456",
          0L,
          "1.0", new long[] { 1, 0 },
          null, null, false,
          null, null, false,
          456L, true)
        ]
        [InlineData(
          "1.0a1",
          0L,
          "1.0", new long[] { 1, 0 },
          "a", 1L, true,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "1.0A1",
          0L,
          "1.0", new long[] { 1, 0 },
          "a", 1L, true,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "1.0.1a1",
          0L,
          "1.0.1", new long[] { 1, 0, 1 },
          "a", 1L, true,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "1.0a2.dev456",
          0L,
          "1.0", new long[] { 1, 0 },
          "a", 2L, true,
          null, null, false,
          456L, true)
        ]
        [InlineData(
          "1.0a12.dev456",
          0L,
          "1.0", new long[] { 1, 0 },
          "a", 12L, true,
          null, null, false,
          456L, true)
        ]
        [InlineData(
          "1.0a12.post456",
          0L,
          "1.0", new long[] { 1, 0 },
          "a", 12L, true,
          "post", 456L, true,
          null, false)
        ]
        [InlineData(
          "1.0a12",
          0L,
          "1.0", new long[] { 1, 0 },
          "a", 12L, true,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "1.0b1.dev456",
          0L,
          "1.0", new long[] { 1, 0 },
          "b", 1L, true,
          null, null, false,
          456L, true)
        ]
        [InlineData(
          "1.0b2",
          0L,
          "1.0", new long[] { 1, 0 },
          "b", 2L, true,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "1.0b2.post345.dev456",
          0L,
          "1.0", new long[] { 1, 0 },
          "b", 2L, true,
          "post", 345L, true,
          456L, true)
        ]
        [InlineData(
          "1.0b2.post345",
          0L,
          "1.0", new long[] { 1, 0 },
          "b", 2L, true,
          "post", 345L, true,
          null, false)
        ]
        [InlineData(
          "1.0rc1",
          0L,
          "1.0", new long[] { 1, 0 },
          "rc", 1L, true,
          null, null, false,
          null, false)
        ]
        [InlineData(
          "1.0rc1.dev456",
          0L,
          "1.0", new long[] { 1, 0 },
          "rc", 1L, true,
          null, null, false,
          456L, true)
        ]
        [InlineData(
          "1.0rc1.post456",
          0L,
          "1.0", new long[] { 1, 0 },
          "rc", 1L, true,
          "post", 456L, true,
          null, false)
        ]
        [InlineData(
          "1.0.post456.dev34",
          0L,
          "1.0", new long[] { 1, 0 },
          null, null, false,
          "post", 456L, true,
          34L, true)
        ]
        [InlineData(
          "1.0.post456",
          0L,
          "1.0", new long[] { 1, 0 },
          null, null, false,
          "post", 456L, true,
          null, false)
        ]
        [InlineData(
          "1.0.POST456",
          0L,
          "1.0", new long[] { 1, 0 },
          null, null, false,
          "post", 456L, true,
          null, false)
        ]
        [InlineData(
          "1.1.dev1",
          0L,
          "1.1", new long[] { 1, 1 },
          null, null, false,
          null, null, false,
          1L, true)
        ]
        [InlineData(
          "1.1.DEV1",
          0L,
          "1.1", new long[] { 1, 1 },
          null, null, false,
          null, null, false,
          1L, true)
        ]
        [InlineData(
          "2020!1.0.1.2.3.4b2.dev456",
          2020L,
          "1.0.1.2.3.4", new long[] { 1, 0, 1, 2, 3, 4 },
          "b", 2L, true,
          null, null, false,
          456L, true)
        ]
        [InlineData(
          "2020!1.0.1.2.3.4.post345.dev456",
          2020L,
          "1.0.1.2.3.4", new long[] { 1, 0, 1, 2, 3, 4 },
          null, null, false,
          "post", 345L, true,
          456L, true)
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
        )
        {
            var info = new PythonVersionInfo(version);
            Assert.Equal(epoch, info.Epoch);
            Assert.Equal(release, info.Release);
            Assert.Equal(releaseParts, info.ReleaseParts);

            if (preReleaseLabel != null)
            {
                Assert.Equal(preReleaseLabel, info.PreRelease.Label);
            }
            else if (info.PreRelease != null)
            {
                Assert.Null(info.PreRelease.Label);
            }
            else
            {
                Assert.Null(info.PreRelease);
            }

            if (preReleaseIncrement != null)
            {
                Assert.Equal(preReleaseIncrement, info.PreRelease.Increment);
            }
            else if (info.PreRelease != null)
            {
                Assert.Null(info.PreRelease.Increment);
            }
            else
            {
                Assert.Null(info.PreRelease);
            }

            Assert.Equal(isPreRelease, info.IsPreRelease);

            if (postReleaseLabel != null)
            {
                Assert.Equal(postReleaseLabel, info.PostRelease.Label);
            }
            else if (info.PostRelease != null)
            {
                Assert.Null(info.PostRelease.Label);
            }
            else
            {
                Assert.Null(info.PostRelease);
            }

            if (postReleaseIncrement != null)
            {
                Assert.Equal(postReleaseIncrement, info.PostRelease.Increment);
            }
            else if (info.PostRelease != null)
            {
                Assert.Null(info.PostRelease.Increment);
            }
            else
            {
                Assert.Null(info.PostRelease);
            }
            Assert.Equal(isPostRelease, info.IsPostRelease);

            if (developmentReleaseIncrement != null)
            {
                Assert.Equal(
                  developmentReleaseIncrement,
                  info.DevelopmentRelease.Increment
                );
            }
            else if (info.DevelopmentRelease != null)
            {
                Assert.Null(info.DevelopmentRelease.Increment);
            }
            else
            {
                Assert.Null(info.DevelopmentRelease);
            }

            Assert.Equal(isDevelopmentRelease, info.IsDevelopmentRelease);
        }

        [Fact]
        public void ParseVersionThrowsExceptionIfVersionIsIncorrectlyFormatted()
        {
            Assert.Throws<VersionParseException>(testCode: () =>
              new PythonVersionInfo("1.0.invalid.format"));
        }

        [Theory]
        [InlineData(
          "1.0.dev456",
          (int)PythonVersionInfo.SuffixType.Development,
          null,
          null)
        ]
        [InlineData(
          "1.0a1",
          (int)PythonVersionInfo.SuffixType.Pre,
          (int)PythonVersionInfo.SuffixType.NoSuffix,
          null)
        ]
        [InlineData(
          "1.0a2.dev456",
          (int)PythonVersionInfo.SuffixType.Pre,
          (int)PythonVersionInfo.SuffixType.Development,
          null)
        ]
        [InlineData(
          "1.0a12.dev456",
          (int)PythonVersionInfo.SuffixType.Pre,
          (int)PythonVersionInfo.SuffixType.Development,
          null)
        ]
        [InlineData(
          "1.0a12",
          (int)PythonVersionInfo.SuffixType.Pre,
          (int)PythonVersionInfo.SuffixType.NoSuffix,
          null)
        ]
        [InlineData(
          "1.0b1.dev456",
          (int)PythonVersionInfo.SuffixType.Pre,
          (int)PythonVersionInfo.SuffixType.Development,
          null)
        ]
        [InlineData(
          "1.0b2",
          (int)PythonVersionInfo.SuffixType.Pre,
          (int)PythonVersionInfo.SuffixType.NoSuffix,
          null)
        ]
        [InlineData(
          "1.0b2.post345.dev456",
          (int)PythonVersionInfo.SuffixType.Pre,
          (int)PythonVersionInfo.SuffixType.Post,
          (int)PythonVersionInfo.SuffixType.Development)
        ]
        [InlineData(
          "1.0b2.post345",
          (int)PythonVersionInfo.SuffixType.Pre,
          (int)PythonVersionInfo.SuffixType.Post,
          (int)PythonVersionInfo.SuffixType.NoSuffix)
        ]
        [InlineData(
          "1.0rc1.dev456",
          (int)PythonVersionInfo.SuffixType.Pre,
          (int)PythonVersionInfo.SuffixType.Development,
          null)
        ]
        [InlineData(
          "1.0rc1",
          (int)PythonVersionInfo.SuffixType.Pre,
          (int)PythonVersionInfo.SuffixType.NoSuffix,
          null)
        ]
        [InlineData(
          "1.0",
          (int)PythonVersionInfo.SuffixType.NoSuffix,
          null,
          null)
        ]
        [InlineData(
          "1.0.post456.dev34",
          (int)PythonVersionInfo.SuffixType.Post,
          null,
          (int)PythonVersionInfo.SuffixType.Development)
        ]
        [InlineData(
          "1.0.post456",
          (int)PythonVersionInfo.SuffixType.Post,
          null,
          (int)PythonVersionInfo.SuffixType.NoSuffix)
        ]

        public void SortPositionsAreCorrectlySet(
          string version,
          int? releaseSortPosition,
          int? preReleaseSortPosition,
          int? postReleaseSortPosition
        )
        {
            var versionInfo = new PythonVersionInfo(version);
            Assert.Equal(releaseSortPosition, (int)versionInfo.ReleaseSuffixType);
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
        [InlineData("1.0rc1", "1.0a2.dev456", 1)]
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
        [InlineData("1.1.dev1", "1.0a12.dev456", 1)]
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
        [InlineData("1.0.post456.dev34", "1.0b2.post345.dev456", 1)]
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
        )
        {
            var left = new PythonVersionInfo(leftVersion);
            var right = new PythonVersionInfo(rightVersion);
            Assert.Equal(expected, left.CompareTo(right));
        }

        [Fact]
        public void CompareToThrowsExceptionIfOtherVersionIsNull()
        {
            var version = new PythonVersionInfo("1.0");
            Assert.Throws<ArgumentException>(testCode: () =>
              version.CompareTo(null));
        }

        [Fact]
        public void CompareToThrowsExceptionIfOtherVersionIsNotPythonVersionInfo()
        {
            var versionInfo = new PythonVersionInfo("1.0");
            var otherVersion = new SemVerVersionInfo("1.0");
            Assert.Throws<ArgumentException>(testCode: () =>
              versionInfo.CompareTo(otherVersion));
        }

        [Fact]
        public void ConvertsToString()
        {
            var now = DateTime.UtcNow;
            var versionInfo = new PythonVersionInfo("1.0.0", now);
            Assert.Equal(
              $"{nameof(versionInfo.Version)}: {versionInfo.Version}, " +
              $"{nameof(versionInfo.DatePublished)}: {versionInfo.DatePublished:O}",
              versionInfo.ToString()
            );
        }
    }
}
