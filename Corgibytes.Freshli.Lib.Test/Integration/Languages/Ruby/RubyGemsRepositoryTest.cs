using System;

using Microsoft.Extensions.DependencyInjection;

using Xunit;
using Xunit.Abstractions;

using Corgibytes.Freshli.Lib.Languages.Ruby;

namespace Corgibytes.Freshli.Lib.Test.Integration.Languages.Ruby
{
    public class RubyGemsRepositoryTest
    {
        private IServiceProvider _serviceProvider;
        private RubyGemsRepository _repository;

        public RubyGemsRepositoryTest(ITestOutputHelper outputHelper)
        {
            _serviceProvider = Fixtures.BuildServiceProviderWith(outputHelper);
            _repository = _serviceProvider.GetRequiredService<RubyGemsRepository>();
        }

        [Fact]
        public async void VersionInfoCorrectlyCreatesVersion()
        {
            var versionInfo = await _repository.VersionInfo("tzinfo", "1.2.7");
            var expectedDate = new DateTime(2020, 04, 02);

            Assert.Equal("1.2.7", versionInfo.Version);
            Assert.Equal(expectedDate, versionInfo.DatePublished);
        }

        [Fact]
        public async void VersionInfoCorrectlyCreatesPreReleaseVersion()
        {
            var versionInfo = await _repository.VersionInfo("git", "1.6.0.pre1");
            var expectedDate = new DateTime(2020, 01, 20);

            Assert.Equal("1.6.0.pre1", versionInfo.Version);
            Assert.Equal(expectedDate, versionInfo.DatePublished);
        }

        [Fact]
        public async void LatestAsOfCorrectlyFindsLatestVersion()
        {
            var targetDate = new DateTime(2020, 02, 01);
            var versionInfo = await _repository.Latest("git", targetDate, false);
            var expectedDate = new DateTime(2018, 08, 10);

            Assert.Equal("1.5.0", versionInfo.Version);
            Assert.Equal(expectedDate, versionInfo.DatePublished);
        }

        [Fact]
        public async void
          LatestAsOfCorrectlyFindsLatestPreReleaseVersion()
        {
            var targetDate = new DateTime(2020, 02, 01);
            var versionInfo = await _repository.Latest("git", targetDate, true);
            var expectedDate = new DateTime(2020, 01, 20);

            Assert.Equal("1.6.0.pre1", versionInfo.Version);
            Assert.Equal(expectedDate, versionInfo.DatePublished);
        }

        [Fact]
        public async void VersionsBetweenFindsVersionsReleasedBeforeTargetDate()
        {
            var targetDate = new DateTime(2014, 04, 01);
            var earlierVersion = new RubyGemsVersionInfo { Version = "0.3.38" };
            var laterVersion = new RubyGemsVersionInfo { Version = "1.1.0" };

            var versions = await _repository.VersionsBetween("tzinfo", targetDate,
              earlierVersion, laterVersion, includePreReleases: true);

            Assert.Equal(3, versions.Count);
        }

        [Fact]
        public async void VersionsBetweenCorrectlyFindsVersions()
        {
            var targetDate = new DateTime(2020, 09, 01);
            var earlierVersion = new RubyGemsVersionInfo { Version = "3.11.0" };
            var laterVersion = new RubyGemsVersionInfo { Version = "3.13.0" };

            var versions = await _repository.VersionsBetween(name: "google-protobuf",
              targetDate, earlierVersion, laterVersion, includePreReleases: false);

            Assert.Equal(8, versions.Count);
        }


        [Fact]
        public async void VersionsBetweenCorrectlyFindsVersionsWithPreReleases()
        {
            var targetDate = new DateTime(2020, 09, 01);
            var earlierVersion = new RubyGemsVersionInfo { Version = "3.11.0" };
            var laterVersion = new RubyGemsVersionInfo { Version = "3.13.0" };

            var versions = await _repository.VersionsBetween(name: "google-protobuf",
              targetDate, earlierVersion, laterVersion, includePreReleases: true);

            Assert.Equal(11, versions.Count);
        }
    }
}
