using System;
using Corgibytes.Freshli.Lib.Exceptions;
using Corgibytes.Freshli.Lib.Languages.CSharp;
using NuGet.Versioning;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration.Languages.CSharp
{
    public class NuGetRepositoryTest
    {
        private NuGetRepository _repository = new NuGetRepository();

        [Fact]
        public void VersionInfo()
        {
            var versionInfo = _repository.VersionInfo("Newtonsoft.Json", "12.0.3");
            var expectedDate =
              new DateTime(2019, 11, 09, 01, 27, 30, 723, DateTimeKind.Utc);

            Assert.Equal("12.0.3", versionInfo.Version);
            Assert.Equal(expectedDate, versionInfo.DatePublished);
        }

        [Fact]
        public void VersionInfoThrowsExceptionIfNotFound()
        {
            Assert.Throws<VersionNotFoundException>(testCode: () =>
              new NuGetRepository().VersionInfo("Newtonsoft.Json", "12.0.900"));
        }

        [Fact]
        public void Latest()
        {
            var asOfDate =
              new DateTime(2019, 11, 10, 01, 27, 30, 723, DateTimeKind.Utc);
            var latest = _repository.Latest("Newtonsoft.Json", asOfDate, false);

            Assert.Equal("12.0.3", latest.Version);
        }

        [Fact]
        public void LatestThrowsExceptionIfNotFound()
        {
            Assert.Throws<LatestVersionNotFoundException>(testCode: () =>
              new NuGetRepository().Latest(
                "Newtonsoft.Json",
                new DateTime(1900, 11, 10, 01, 27, 30, 723, DateTimeKind.Utc),
                false
              )
            );
        }

        [Fact]
        public void VersionsBetween()
        {
            var asOfDate =
              new DateTime(2019, 11, 10, 01, 27, 30, 723, DateTimeKind.Utc);
            var versionsBetween = _repository.VersionsBetween(
              "Newtonsoft.Json",
              asOfDate,
              new FreshliNuGetVersionInfo(
                new NuGetVersion("11.0.2"), DateTime.UtcNow),
              new FreshliNuGetVersionInfo(
                new NuGetVersion("12.0.3"), DateTime.UtcNow),
              false
            );

            Assert.Equal(2, versionsBetween.Count);
            Assert.Equal("12.0.2", versionsBetween[0].Version);
            Assert.Equal("12.0.1", versionsBetween[1].Version);
        }
    }
}
