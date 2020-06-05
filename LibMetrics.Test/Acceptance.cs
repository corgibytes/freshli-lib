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
    public Acceptance() {
      ManifestFinder.Register<RubyBundlerManifestFinder>();
      ManifestFinder.Register<PhpComposerManifestFinder>();
      ManifestFinder.Register<PhpComposerManifestFinder>();

      FileHistoryFinder.Register<GitFileHistoryFinder>();
    }

    private DateTime _testingBoundary =
      new DateTime(2020, 01, 01, 0, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void RubyGemsWithGitHistory()
    {
      var runner = new Runner();

      var rubyFixturePath = Fixtures.Path("ruby", "nokotest");
      var results = runner.Run(rubyFixturePath, asOf: _testingBoundary);

      Approvals.VerifyAll(results, "results");
    }

    [Fact]
    public void RubyGemsWithHistoryViaGitHub() {
      var runner = new Runner();

      var repoUrl =
        "https://github.com/corgibytes/libmetrics-fixture-ruby-nokotest";
      var results = runner.Run(repoUrl, asOf: _testingBoundary);

      Approvals.VerifyAll(results, "results");
    }

    [Fact]
    public void RubyGemsFeedbinHistoryViaGitHub()
    {
      var runner = new Runner();

      var repoUrl = "https://github.com/feedbin/feedbin";
      var results = runner.Run(repoUrl, asOf: _testingBoundary);

      Approvals.VerifyAll(results, "results");
    }

    [Fact]
    public void ComposerWithoutGitHistory()
    {
      var runner = new Runner();

      var phpFixturePath = Fixtures.Path("php", "large");
      var results = runner.Run(phpFixturePath, asOf: _testingBoundary);

      Approvals.VerifyAll(results, "results");
    }

    [Fact]
    public void DrupalComposerWithoutGitHistory()
    {
      var runner = new Runner();

      var phpFixturePath = Fixtures.Path("php", "drupal");
      var results = runner.Run(phpFixturePath, asOf: _testingBoundary);

      Approvals.VerifyAll(results, "results");
    }

    [Fact]
    public void RequirementsTxtPyspider() {
      var runner = new Runner();

      var results = runner.Run(
        "https://github.com/binux/pyspider",
        asOf: _testingBoundary
      );

      Approvals.VerifyAll(results, "results");
    }
  }
}
