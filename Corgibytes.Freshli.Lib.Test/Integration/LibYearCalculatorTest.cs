using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using Xunit;
using VerifyTests;
using VerifyXunit;

using Corgibytes.Freshli.Lib.Languages.Ruby;
using System.Threading.Tasks;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    [UsesVerify]
    public class LibYearCalculatorTest
    {
        private RubyGemsRepository _repository = new();
        private List<PackageInfo> _packages = new();
        private LibYearCalculator _calculator;

        public LibYearCalculatorTest()
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(LoggerRecording.Start());
            var logger = loggerFactory.CreateLogger<LibYearCalculator>();
            _calculator = new LibYearCalculator(_repository, _packages, true, logger);
        }

        [Fact]
        public Task ComputeAsOf()
        {
            BuildBundlerManifestWithMiniPortileAndNokogiri(
                miniPortileVersion: "2.1.0",
                nokogiriVersion: "1.7.0"
            );

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2017, 04, 01, 00, 00, 00, TimeSpan.Zero)
            );

            return Verifier.Verify(results);
        }

        [Fact]
        public Task ComputeAsOfSmallValue()
        {
            BuildBundlerManifestWithMiniPortileAndNokogiri(
                miniPortileVersion: "2.1.0",
                nokogiriVersion: "1.7.0"
            );

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2017, 02, 01, 00, 00, 00, TimeSpan.Zero)
            );

            return Verifier.Verify(results);
        }

        [Fact]
        public Task ComputeAsOfEdgeCase()
        {
            BuildBundlerManifestWithMiniPortileAndNokogiri();

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2018, 01, 01, 00, 00, 00, TimeSpan.Zero)
            );

            return Verifier.Verify(results);
        }

        [Fact]
        public Task ComputeAsOfOtherEdgeCase()
        {
            BuildBundlerManifestWithMiniPortileAndNokogiri();

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2018, 02, 01, 00, 00, 00, TimeSpan.FromHours(-5))
            );

            return Verifier.Verify(results);
        }

        [Fact]
        public Task ComputeAsOfWithNoNewerMinorReleases()
        {
            BuildBundlerManifest(new Dictionary<string, string>
            {
                {"tzinfo", "0.3.38"}
            });

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2014, 03, 01, 00, 00, 00, TimeSpan.Zero)
            );

            return Verifier.Verify(results);
        }

        [Fact]
        public Task ComputeAsOfWithNewerMinorReleases()
        {
            BuildBundlerManifest(new Dictionary<string, string>
            {
                {"tzinfo", "0.3.38"}
            });

            var results = _calculator.ComputeAsOf(
              new DateTimeOffset(2014, 04, 01, 00, 00, 00, TimeSpan.Zero)
            );

            return Verifier.Verify(results);
        }

        [Fact]
        public Task ComputeAsOfWithPreReleaseVersion()
        {
            BuildBundlerManifest(new Dictionary<string, string>
            {
                {"google-protobuf", "3.12.0.rc.1"}
            });

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2020, 06, 01, 00, 00, 00, TimeSpan.Zero)
            );

            return Verifier.Verify(results);
        }

        [Fact]
        public Task ComputeAsOfWithLatestVersionBeingPreReleaseVersion()
        {
            BuildBundlerManifest(new Dictionary<string, string>
            {
                {"google-protobuf", "3.10.0.rc.1"}
            });

            var results = _calculator.ComputeAsOf(
                new DateTimeOffset(2019, 11, 25, 00, 00, 00, TimeSpan.FromHours(-5))
            );

            return Verifier.Verify(results);
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
            _packages.Clear();
            foreach (var entry in packagesAndVersions)
            {
                _packages.Add(new PackageInfo(entry.Key, entry.Value));
            }
        }
    }
}
