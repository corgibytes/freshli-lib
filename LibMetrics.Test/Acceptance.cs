using System;
using System.Globalization;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalTests.Reporters.TestFrameworks;
using LibMetrics.Languages.Php;
using LibMetrics.Languages.Ruby;
using Xunit;

namespace LibMetrics.Test
{
  [UseReporter(typeof(XUnit2Reporter))]
  public class Acceptance
  {
    private static DateTime ParseExact(string value)
    {
      return DateTime.ParseExact(value, "o", CultureInfo.InvariantCulture,
        DateTimeStyles.RoundtripKind);
    }

    private DateTime _testingBoundary = ParseExact(
      "2020-01-01T00:00:00.0000000Z");

    [Fact]
    public void RubyGemsWithGitHistory()
    {
      ManifestFinder.Register<RubyBundlerManifestFinder>();
      FileHistoryFinder.Register<GitFileHistoryFinder>();

      var runner = new Runner();

      var rubyFixturePath = Fixtures.Path("ruby", "nokotest");
      var results = runner.Run(rubyFixturePath, asOf: _testingBoundary);

      Approvals.VerifyAll(results, "results");
    }

    [Fact]
    public void RubyGemsWithHistoryViaGitHub() {
      ManifestFinder.Register<RubyBundlerManifestFinder>();
      FileHistoryFinder.Register<GitFileHistoryFinder>();

      var runner = new Runner();

      var repoUrl =
        "https://github.com/corgibytes/libmetrics-fixture-ruby-nokotest";
      var results = runner.Run(repoUrl, asOf: _testingBoundary);

      Approvals.VerifyAll(results, "results");
    }

    [Fact]
    public void RubyGemsFeedbinHistoryViaGitHub()
    {
      ManifestFinder.Register<RubyBundlerManifestFinder>();
      FileHistoryFinder.Register<GitFileHistoryFinder>();

      var runner = new Runner();

      var repoUrl = "https://github.com/feedbin/feedbin";
      var results = runner.Run(repoUrl, asOf: _testingBoundary);

      Approvals.VerifyAll(results, "results");
    }

    [Fact]
    public void ComposerWithoutGitHistory()
    {
      ManifestFinder.Register<PhpComposerManifestFinder>();

      var runner = new Runner();

      var phpFixturePath = Fixtures.Path("php", "large");
      var results = runner.Run(phpFixturePath, asOf: _testingBoundary);

      Approvals.VerifyAll(results, "results");
    }

    [Fact]
    public void DrupalComposerWithoutGitHistory()
    {
      ManifestFinder.Register<PhpComposerManifestFinder>();

      var runner = new Runner();

      var phpFixturePath = Fixtures.Path("php", "drupal");
      var results = runner.Run(phpFixturePath, asOf: _testingBoundary);

      Approvals.VerifyAll(results, "results");
    }
  }
}
