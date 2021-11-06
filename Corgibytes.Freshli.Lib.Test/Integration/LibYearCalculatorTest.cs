using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Corgibytes.Freshli.Lib.Languages.Ruby;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    public class LibYearCalculatorTest
    {
        private RubyGemsRepository _repository = new();
        private BundlerManifest _manifest = new();
        private LibYearCalculator _calculator;

        public LibYearCalculatorTest()
        {
            _calculator = new LibYearCalculator(_repository, _manifest);
        }

        [Fact]
        public void ComputeAsOf()
        {
            BuildBundlerManifestWithMiniPortileAndNokogiri(
                miniPortileVersion: "2.1.0",
                nokogiriVersion: "1.7.0"
            );

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2017, 04, 01, 00, 00, 00, TimeSpan.Zero)
            );

            Assert.Equal(0.227, results.Total, 3);

            Assert.Equal(0.0, results["mini_portile2"].Value, 3);
            Assert.Equal("2.1.0", results["mini_portile2"].Version);
            Assert.Equal(
                new DateTimeOffset(2016, 01, 06, 19, 10, 42, TimeSpan.Zero),
                results["mini_portile2"].PublishedAt
            );
            Assert.Equal("2.1.0", results["mini_portile2"].LatestVersion);
            Assert.Equal(
                new DateTimeOffset(2016, 01, 06, 19, 10, 42, TimeSpan.Zero),
                results["mini_portile2"].LatestPublishedAt
            );
            Assert.False(results["mini_portile2"].UpgradeAvailable);
            Assert.Equal(0.227, results["nokogiri"].Value, 3);
            Assert.Equal("1.7.0", results["nokogiri"].Version);
            Assert.Equal(
                new DateTimeOffset(2016, 12, 27, 03, 49, 28, TimeSpan.Zero),
                results["nokogiri"].PublishedAt);
            Assert.Equal("1.7.1", results["nokogiri"].LatestVersion);
            Assert.Equal(
                new DateTimeOffset(2017, 03, 20, 03, 39, 14, TimeSpan.Zero),
                results["nokogiri"].LatestPublishedAt);
            Assert.True(results["nokogiri"].UpgradeAvailable);
        }

        [Fact]
        public void ComputeAsOfSmallValue()
        {
            BuildBundlerManifestWithMiniPortileAndNokogiri(
                miniPortileVersion: "2.1.0",
                nokogiriVersion: "1.7.0"
            );

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2017, 02, 01, 00, 00, 00, TimeSpan.Zero)
            );

            Assert.Equal(0.022, results.Total, 3);

            Assert.Equal(0, results["mini_portile2"].Value, 3);
            Assert.Equal("2.1.0", results["mini_portile2"].LatestVersion);
            Assert.False(results["mini_portile2"].UpgradeAvailable);
            Assert.Equal(0.022, results["nokogiri"].Value, 3);
            Assert.Equal("1.7.0.1", results["nokogiri"].LatestVersion);
            Assert.True(results["nokogiri"].UpgradeAvailable);
        }

        [Fact]
        public void ComputeAsOfEdgeCase()
        {
            BuildBundlerManifestWithMiniPortileAndNokogiri();

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2018, 01, 01, 00, 00, 00, TimeSpan.Zero)
            );

            Assert.Equal(0, results.Total, 3);

            Assert.Equal(0, results["mini_portile2"].Value, 3);
            Assert.Equal("2.3.0", results["mini_portile2"].LatestVersion);
            Assert.False(results["mini_portile2"].UpgradeAvailable);
            Assert.Equal(0, results["nokogiri"].Value, 3);
            Assert.Equal("1.8.1", results["nokogiri"].LatestVersion);
            Assert.False(results["nokogiri"].UpgradeAvailable);
        }

        [Fact]
        public void ComputeAsOfOtherEdgeCase()
        {
            BuildBundlerManifestWithMiniPortileAndNokogiri();

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2018, 02, 01, 00, 00, 00, TimeSpan.FromHours(-5))
            );

            Assert.Equal(0.361, results.Total, 3);

            Assert.Equal(0.0, results["mini_portile2"].Value, 3);
            Assert.Equal("2.3.0", results["mini_portile2"].LatestVersion);
            Assert.False(results["mini_portile2"].UpgradeAvailable);
            Assert.Equal(0.361, results["nokogiri"].Value, 3);
            Assert.Equal("1.8.2", results["nokogiri"].LatestVersion);
            Assert.True(results["nokogiri"].UpgradeAvailable);
        }

        [Fact]
        public void ComputeAsOfWithNoNewerMinorReleases()
        {
            BuildBundlerManifest(new Dictionary<string, string>
            {
                {"tzinfo", "0.3.38"}
            });

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2014, 03, 01, 00, 00, 00, TimeSpan.Zero)
            );

            Assert.Equal(0, results.Total);
            Assert.Equal(0, results["tzinfo"].Value);
            Assert.Equal("1.1.0", results["tzinfo"].LatestVersion);
            Assert.True(results["tzinfo"].UpgradeAvailable);
        }

        [Fact]
        public void ComputeAsOfWithNewerMinorReleases()
        {
            BuildBundlerManifest(new Dictionary<string, string>
            {
                {"tzinfo", "0.3.38"}
            });

            var results = _calculator.ComputeAsOf(
              new DateTimeOffset(2014, 04, 01, 00, 00, 00, TimeSpan.Zero)
            );

            Assert.Equal(0.416, results.Total, 3);
            Assert.Equal(0.416, results["tzinfo"].Value, 3);
            Assert.Equal("0.3.39", results["tzinfo"].LatestVersion);
            Assert.True(results["tzinfo"].UpgradeAvailable);
        }

        [Fact]
        public void ComputeAsOfWithPreReleaseVersion()
        {
            BuildBundlerManifest(new Dictionary<string, string>
            {
                {"google-protobuf", "3.12.0.rc.1"}
            });

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2020, 06, 01, 00, 00, 00, TimeSpan.Zero)
            );

            Assert.Equal(0.063, results.Total, 3);
            Assert.Equal(0.063, results["google-protobuf"].Value, 3);
            Assert.Equal("3.12.0.rc.1", results["google-protobuf"].Version);
            Assert.Equal(
                new DateTimeOffset(2020, 05, 04, 22, 46, 23, TimeSpan.Zero),
                results["google-protobuf"].PublishedAt
            );
            Assert.Equal("3.12.2", results["google-protobuf"].LatestVersion);
            Assert.Equal(
                new DateTimeOffset(2020, 05, 27, 18, 50, 26, TimeSpan.Zero),
                results["google-protobuf"].LatestPublishedAt
            );
            Assert.True(results["google-protobuf"].UpgradeAvailable);
        }

        [Fact]
        public void ComputeAsOfWithLatestVersionBeingPreReleaseVersion()
        {
            BuildBundlerManifest(new Dictionary<string, string>
            {
                {"google-protobuf", "3.10.0.rc.1"}
            });

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2019, 11, 25, 00, 00, 00, TimeSpan.FromHours(-5))
            );

            Assert.Equal(0.214, results.Total, 3);
            Assert.Equal(0.214, results["google-protobuf"].Value, 3);
            Assert.Equal("3.10.0.rc.1", results["google-protobuf"].Version);
            Assert.Equal(
                new DateTimeOffset(2019, 09, 05, 19, 43, 14, TimeSpan.Zero),
                results["google-protobuf"].PublishedAt
            );
            Assert.Equal("3.11.0.rc.2", results["google-protobuf"].LatestVersion);
            Assert.Equal(
                new DateTimeOffset(2019, 11, 23, 00, 10, 38, TimeSpan.Zero),
                results["google-protobuf"].LatestPublishedAt
            );
            Assert.True(results["google-protobuf"].UpgradeAvailable);
        }

        private void BuildBundlerManifestWithMiniPortileAndNokogiri(
            string miniPortileVersion = "2.3.0", string nokogiriVersion = "1.8.1")
        {
            BuildBundlerManifest(new Dictionary<string, string>
            {
                {"mini_portile2", miniPortileVersion}, {"nokogiri", nokogiriVersion}
            });
        }

        private void BuildBundlerManifest(
            Dictionary<string, string> packagesAndVersions
        )
        {
            _manifest.Clear();
            foreach (var entry in packagesAndVersions)
            {
                _manifest.Add(entry.Key, entry.Value);
            }
        }
    }
}
