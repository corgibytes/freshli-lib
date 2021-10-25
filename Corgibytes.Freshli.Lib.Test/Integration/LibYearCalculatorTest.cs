using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using HtmlAgilityPack;

using Polly;
using Polly.Registry;

using Xunit;
using Xunit.Abstractions;

using Corgibytes.Freshli.Lib.Languages.Ruby;


namespace Corgibytes.Freshli.Lib.Test.Integration
{
    public class LibYearCalculatorTest
    {
        RubyGemsRepository _repository;
        IServiceProvider _serviceProvider;


        public LibYearCalculatorTest(ITestOutputHelper outputHelper)
        {
            _serviceProvider = Fixtures.BuildServiceProviderWith(outputHelper);
            _repository = _serviceProvider.GetRequiredService<RubyGemsRepository>();
        }

        [Fact]
        public async void ComputeAsOf()
        {
            var manifest = new BundlerManifest();
            manifest.Add("mini_portile2", "2.1.0");
            manifest.Add("nokogiri", "1.7.0");

            var calculator = new LibYearCalculator(_repository, manifest);

            var results = await calculator.ComputeAsOf(new DateTime(2017, 04, 01));

            Assert.Equal(0.227, results.Total, 3);

            Assert.Equal(0.0, results["mini_portile2"].Value, 3);
            Assert.Equal("2.1.0", results["mini_portile2"].Version);
            Assert.Equal(new DateTime(2016, 01, 06), results["mini_portile2"].PublishedAt);
            Assert.Equal("2.1.0", results["mini_portile2"].LatestVersion);
            Assert.Equal(new DateTime(2016, 01, 06), results["mini_portile2"].LatestPublishedAt);
            Assert.False(results["mini_portile2"].UpgradeAvailable);
            Assert.Equal(0.227, results["nokogiri"].Value, 3);
            Assert.Equal("1.7.0", results["nokogiri"].Version);
            Assert.Equal(new DateTime(2016, 12, 27), results["nokogiri"].PublishedAt);
            Assert.Equal("1.7.1", results["nokogiri"].LatestVersion);
            Assert.Equal(new DateTime(2017, 03, 20), results["nokogiri"].LatestPublishedAt);
            Assert.True(results["nokogiri"].UpgradeAvailable);
        }

        [Fact]
        public async void ComputeAsOfSmallValue()
        {
            var manifest = new BundlerManifest();
            manifest.Add("mini_portile2", "2.1.0");
            manifest.Add("nokogiri", "1.7.0");

            var calculator = new LibYearCalculator(_repository, manifest);

            var results = await calculator.ComputeAsOf(new DateTime(2017, 02, 01));

            Assert.Equal(0.022, results.Total, 3);

            Assert.Equal(0, results["mini_portile2"].Value, 3);
            Assert.Equal("2.1.0", results["mini_portile2"].LatestVersion);
            Assert.False(results["mini_portile2"].UpgradeAvailable);
            Assert.Equal(0.022, results["nokogiri"].Value, 3);
            Assert.Equal("1.7.0.1", results["nokogiri"].LatestVersion);
            Assert.True(results["nokogiri"].UpgradeAvailable);
        }

        [Fact]
        public async void ComputeAsOfEdgeCase()
        {
            var manifest = new BundlerManifest();
            manifest.Add("mini_portile2", "2.3.0");
            manifest.Add("nokogiri", "1.8.1");

            var calculator = new LibYearCalculator(_repository, manifest);

            var results = await calculator.ComputeAsOf(new DateTime(2018, 01, 01));

            Assert.Equal(0, results.Total, 3);

            Assert.Equal(0, results["mini_portile2"].Value, 3);
            Assert.Equal("2.3.0", results["mini_portile2"].LatestVersion);
            Assert.False(results["mini_portile2"].UpgradeAvailable);
            Assert.Equal(0, results["nokogiri"].Value, 3);
            Assert.Equal("1.8.1", results["nokogiri"].LatestVersion);
            Assert.False(results["nokogiri"].UpgradeAvailable);
        }

        [Fact]
        public async void ComputeAsOfOtherEdgeCase()
        {
            var manifest = new BundlerManifest();
            manifest.Add("mini_portile2", "2.3.0");
            manifest.Add("nokogiri", "1.8.1");

            var calculator = new LibYearCalculator(_repository, manifest);

            var results = await calculator.ComputeAsOf(new DateTime(2018, 02, 01));

            Assert.Equal(0.362, results.Total, 3);

            Assert.Equal(0.0, results["mini_portile2"].Value, 3);
            Assert.Equal("2.3.0", results["mini_portile2"].LatestVersion);
            Assert.False(results["mini_portile2"].UpgradeAvailable);
            Assert.Equal(0.362, results["nokogiri"].Value, 3);
            Assert.Equal("1.8.2", results["nokogiri"].LatestVersion);
            Assert.True(results["nokogiri"].UpgradeAvailable);
        }

        [Fact]
        public async void ComputeAsOfWithNoNewerMinorReleases()
        {
            var manifest = new BundlerManifest();
            manifest.Add("tzinfo", "0.3.38");
            var calculator = new LibYearCalculator(_repository, manifest);

            var results = await calculator.ComputeAsOf(new DateTime(2014, 03, 01));

            Assert.Equal(0, results.Total);
            Assert.Equal(0, results["tzinfo"].Value);
            Assert.Equal("1.1.0", results["tzinfo"].LatestVersion);
            Assert.True(results["tzinfo"].UpgradeAvailable);
        }

        [Fact]
        public async void ComputeAsOfWithNewerMinorReleases()
        {
            var manifest = new BundlerManifest();
            manifest.Add("tzinfo", "0.3.38");
            var calculator = new LibYearCalculator(_repository, manifest);

            var results = await calculator.ComputeAsOf(new DateTime(2014, 04, 01));

            Assert.Equal(0.416, results.Total, 3);
            Assert.Equal(0.416, results["tzinfo"].Value, 3);
            Assert.Equal("0.3.39", results["tzinfo"].LatestVersion);
            Assert.True(results["tzinfo"].UpgradeAvailable);
        }

        [Fact]
        public async void ComputeAsOfWithPreReleaseVersion()
        {
            var manifest = new BundlerManifest();
            manifest.Add("google-protobuf", "3.12.0.rc.1");
            var calculator = new LibYearCalculator(_repository, manifest);

            var results = await calculator.ComputeAsOf(new DateTime(2020, 06, 01));

            Assert.Equal(0.063, results.Total, 3);
            Assert.Equal(0.063, results["google-protobuf"].Value, 3);
            Assert.Equal("3.12.0.rc.1", results["google-protobuf"].Version);
            Assert.Equal(new DateTime(2020, 05, 04), results["google-protobuf"].PublishedAt);
            Assert.Equal("3.12.2", results["google-protobuf"].LatestVersion);
            Assert.Equal(new DateTime(2020, 05, 27), results["google-protobuf"].LatestPublishedAt);
            Assert.True(results["google-protobuf"].UpgradeAvailable);
        }

        [Fact]
        public async void ComputeAsOfWithLatestVersionBeingPreReleaseVersion()
        {
            var manifest = new BundlerManifest();
            manifest.Add("google-protobuf", "3.10.0.rc.1");
            var calculator = new LibYearCalculator(_repository, manifest);

            var results = await calculator.ComputeAsOf(new DateTime(2019, 11, 25));

            Assert.Equal(0.216, results.Total, 3);
            Assert.Equal(0.216, results["google-protobuf"].Value, 3);
            Assert.Equal("3.10.0.rc.1", results["google-protobuf"].Version);
            Assert.Equal(new DateTime(2019, 09, 05), results["google-protobuf"].PublishedAt);
            Assert.Equal("3.11.0.rc.2", results["google-protobuf"].LatestVersion);
            Assert.Equal(new DateTime(2019, 11, 23), results["google-protobuf"].LatestPublishedAt);
            Assert.True(results["google-protobuf"].UpgradeAvailable);
        }

        [Fact]
        public void Compute()
        {
            var olderVersion = new SemVerVersionInfo("1.7.0", new DateTime(2016, 12, 27));
            var newerVersion = new SemVerVersionInfo("1.7.1", new DateTime(2017, 03, 20));

            Assert.Equal(0.227, LibYearCalculator.Compute(olderVersion, newerVersion), 3);
        }
    }
}
