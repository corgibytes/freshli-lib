using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using LibMetrics.Languages.Php;
using LibMetrics.Languages.Ruby;
using Xunit;

namespace LibMetrics.Test
{
  [UseReporter(typeof(DiffReporter))]
  public class Acceptance
  {
    [Fact]
    public void RubyGemsWithGitHistory()
    {
      ManifestFinder.Register<RubyBundlerManifestFinder>();
      FileHistoryFinder.Register<GitFileHistoryFinder>();

      var runner = new Runner();

      var rubyFixturePath = Fixtures.Path("ruby", "nokotest");
      var results = runner.Run(
        rubyFixturePath,
        asOf: new DateTime(2020, 01, 01));

      Approvals.VerifyAll(results, "results");
    }

    [Fact]
    public void RubyGemsWithHistoryViaGitHub() {
      ManifestFinder.Register<RubyBundlerManifestFinder>();
      FileHistoryFinder.Register<GitFileHistoryFinder>();

      var runner = new Runner();

      var repoUrl =
        "https://github.com/corgibytes/libmetrics-fixture-ruby-nokotest";
      var results = runner.Run(repoUrl, asOf: new DateTime(2020, 01, 01));

      Approvals.VerifyAll(results, "results");
    }

    [Fact]
    public void RubyGemsFeedbinHistoryViaGitHub()
    {
      ManifestFinder.Register<RubyBundlerManifestFinder>();
      FileHistoryFinder.Register<GitFileHistoryFinder>();

      var runner = new Runner();

      var repoUrl = "https://github.com/feedbin/feedbin";
      var results = runner.Run(repoUrl, asOf: new DateTime(2020, 01, 01));

      Approvals.VerifyAll(results, "results");
    }

    [Fact]
    public void ComposerWithoutGitHistory()
    {
      ManifestFinder.Register<PhpComposerManifestFinder>();

      var runner = new Runner();

      var phpFixturePath = Fixtures.Path("php", "large");
      var results = runner.Run(
        phpFixturePath,
        asOf: new DateTime(2020, 01, 01));

      Approvals.VerifyAll(results, "results");
    }

    [Fact]
    public void DrupalComposerWithoutGitHistory()
    {
      ManifestFinder.Register<PhpComposerManifestFinder>();

      var runner = new Runner();

      var phpFixturePath = Fixtures.Path("php", "drupal");
      var results = runner.Run(
        phpFixturePath,
        asOf: new DateTime(2020, 01, 01));

      Approvals.VerifyAll(results, "results");
    }

    }
  }
}
