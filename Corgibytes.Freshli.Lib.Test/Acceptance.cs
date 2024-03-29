using System;
using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test
{
    [UsesVerify]
    public class Acceptance
    {
        public Acceptance()
        {
        }

        private DateTimeOffset _testingBoundary =
          new DateTimeOffset(2020, 02, 01, 0, 0, 0, 0, TimeSpan.Zero);

        [Fact]
        public Task RubyGemsWithGitHistory()
        {
            var runner = new Runner();

            var rubyFixturePath = Fixtures.Path("ruby", "nokotest");
            var results = runner.Run(rubyFixturePath, asOf: _testingBoundary);

            return Verifier.Verify(results);
        }

        [Fact]
        public Task RubyGemsWithHistoryViaGitHub()
        {
            var runner = new Runner();

            var repoUrl =
              "https://github.com/corgibytes/freshli-fixture-ruby-nokotest";
            var results = runner.Run(repoUrl, asOf: _testingBoundary);

            return Verifier.Verify(results);
        }

        [Fact]
        public Task RubyGemsFeedbinHistoryViaGitHub()
        {
            var runner = new Runner();

            var repoUrl = "https://github.com/feedbin/feedbin";
            var results = runner.Run(repoUrl, asOf: _testingBoundary);

            return Verifier.Verify(results);
        }

        [Fact]
        public Task RubyGemsClearanceHistoryViaGitHub()
        {
            var runner = new Runner();
            var results = runner.Run(
              "https://github.com/thoughtbot/clearance",
              asOf: new DateTimeOffset(2020, 06, 01, 00, 00, 00, 00, TimeSpan.Zero)
            );

            return Verifier.Verify(results);
        }

        [Fact]
        public Task ComposerWithoutGitHistory()
        {
            var runner = new Runner();

            var phpFixturePath = Fixtures.Path("php", "large");
            var results = runner.Run(phpFixturePath, asOf: _testingBoundary);

            return Verifier.Verify(results);
        }

        [Fact]
        public Task DrupalComposerWithoutGitHistory()
        {
            var runner = new Runner();

            var phpFixturePath = Fixtures.Path("php", "drupal");
            var results = runner.Run(phpFixturePath, asOf: _testingBoundary);

            return Verifier.Verify(results);
        }

        [Fact]
        public Task RequirementsTxtPyspider()
        {
            var runner = new Runner();

            var results = runner.Run(
              "https://github.com/binux/pyspider",
              asOf: _testingBoundary
            );

            return Verifier.Verify(results);
        }

        [Fact]
        public Task CpanfileDancer2()
        {
            var runner = new Runner();

            var results = runner.Run(
              "https://github.com/PerlDancer/Dancer2",
              asOf: _testingBoundary
            );

            return Verifier.Verify(results);
        }

        [Fact]
        public Task SpaCyWithHistoryViaGitHub()
        {
            var runner = new Runner();

            var results = runner.Run(
                "https://github.com/explosion/spaCy",
                asOf: new DateTimeOffset(2017, 6, 1, 0, 0, 0, TimeSpan.Zero)
            );

            return Verifier.Verify(results);
        }

        [Fact]
        public Task UnsupportedGitRepository()
        {
            var runner = new Runner();

            var results = runner.Run(
              "https://github.com/gohugoio/hugo",
              asOf: _testingBoundary
            );

            return Verifier.Verify(results);
        }
    }
}
