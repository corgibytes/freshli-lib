using System;
using ApprovalTests;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test
{
    public class Acceptance
    {

        private DateTimeOffset _testingBoundary =
          new DateTimeOffset(2020, 02, 01, 0, 0, 0, 0, TimeSpan.Zero);

        [Fact]
        public void RubyGemsWithGitHistory()
        {
            var runner = new Runner();

            var rubyFixturePath = Fixtures.Path("ruby", "nokotest");
            var results = runner.Run(rubyFixturePath, asOf: _testingBoundary);

            Assert.True(runner.ManifestFinder.Successful);
            Approvals.VerifyAll(results, "results");
        }

        [Fact]
        public void RubyGemsWithHistoryViaGitHub()
        {
            var runner = new Runner();

            var repoUrl =
              "https://github.com/corgibytes/freshli-fixture-ruby-nokotest";
            var results = runner.Run(repoUrl, asOf: _testingBoundary);

            Assert.True(runner.ManifestFinder.Successful);
            Approvals.VerifyAll(results, "results");
        }

        [Fact]
        public void RubyGemsFeedbinHistoryViaGitHub()
        {
            var runner = new Runner();

            var repoUrl = "https://github.com/feedbin/feedbin";
            var results = runner.Run(repoUrl, asOf: _testingBoundary);

            Assert.True(runner.ManifestFinder.Successful);
            Approvals.VerifyAll(results, "results");
        }

        [Fact]
        public void RubyGemsClearanceHistoryViaGitHub()
        {
            var runner = new Runner();
            var results = runner.Run(
              "https://github.com/thoughtbot/clearance",
              asOf: new DateTimeOffset(2020, 06, 01, 00, 00, 00, 00, TimeSpan.Zero)
            );

            Assert.True(runner.ManifestFinder.Successful);
            Approvals.VerifyAll(results, "results");
        }

        [Fact]
        public void ComposerWithoutGitHistory()
        {
            var runner = new Runner();

            var phpFixturePath = Fixtures.Path("php", "large");
            var results = runner.Run(phpFixturePath, asOf: _testingBoundary);

            Assert.True(runner.ManifestFinder.Successful);
            Approvals.VerifyAll(results, "results");
        }

        [Fact]
        public void DrupalComposerWithoutGitHistory()
        {
            var runner = new Runner();

            var phpFixturePath = Fixtures.Path("php", "drupal");
            var results = runner.Run(phpFixturePath, asOf: _testingBoundary);

            Assert.True(runner.ManifestFinder.Successful);
            Approvals.VerifyAll(results, "results");
        }

        [Fact]
        public void RequirementsTxtPyspider()
        {
            var runner = new Runner();

            var results = runner.Run(
              "https://github.com/binux/pyspider",
              asOf: _testingBoundary
            );

            Assert.True(runner.ManifestFinder.Successful);
            Approvals.VerifyAll(results, "results");
        }

        [Fact]
        public void CpanfileDancer2()
        {
            var runner = new Runner();

            var results = runner.Run(
              "https://github.com/PerlDancer/Dancer2",
              asOf: _testingBoundary
            );

            Assert.True(runner.ManifestFinder.Successful);
            Approvals.VerifyAll(results, "results");
        }

        [Fact]
        public void SpaCyWithHistoryViaGitHub()
        {
            var runner = new Runner();

            var results = runner.Run(
              "https://github.com/explosion/spaCy",
              asOf: new DateTimeOffset(2017, 06, 01, 00, 00, 00, TimeSpan.Zero)
            );

            Assert.True(runner.ManifestFinder.Successful);
            Approvals.VerifyAll(results, "results");
        }

        [Fact]
        public void UnsupportedGitRepository()
        {
            var runner = new Runner();

            var results = runner.Run(
              "https://github.com/gohugoio/hugo",
              asOf: _testingBoundary
            );

            Assert.False(runner.ManifestFinder.Successful);
            Approvals.VerifyAll(results, "results");
        }
    }
}
