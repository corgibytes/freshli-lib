using System;
using System.Collections.Generic;
using System.IO;
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
      var results = runner.Run(rubyFixturePath);

      AssertNokoFixtureRubyGemsResults(results);
    }

    [Fact]
    public void RubyGemsWithHistoryViaGitHub() {
      ManifestFinder.Register<RubyBundlerManifestFinder>();
      FileHistoryFinder.Register<GitFileHistoryFinder>();

      var runner = new Runner();

      var repoUrl =
        "https://github.com/corgibytes/libmetrics-fixture-ruby-nokotest";
      var results = runner.Run(repoUrl);

      AssertNokoFixtureRubyGemsResults(results);
    }

    [Fact]
    public void RubyGemsFeedbinHistoryViaGitHub()
    {
      ManifestFinder.Register<RubyBundlerManifestFinder>();
      FileHistoryFinder.Register<GitFileHistoryFinder>();

      var runner = new Runner();

      var repoUrl = "https://github.com/feedbin/feedbin";
      var results = runner.Run(repoUrl);

      var output = new StringWriter();

      foreach (var resultSet in results) {
        if (resultSet.Date <= new DateTime(2020, 01, 01)) {
          output.WriteLine(
            $"{resultSet.Date.ToString("yyyy/MM/dd")}: " +
            $"{resultSet.LibYear.Total.ToString("F3")}"
          );
        }
      }

      Approvals.Verify(output);
    }

    private static void AssertNokoFixtureRubyGemsResults(IList<MetricsResult> results) {
      // nokotest contains a git repository with the following history:
      // Gemfile v1 created on 2017/01/01 references nokogiri 1.7.0
      // Gemfile v2 created on 2018/01/01 references nokogiri 1.8.1
      // Gemfile v3 created on 2019/01/01 references nokogiri 1.9.1

      var output = new StringWriter();

      foreach (var resultSet in results) {
          if (resultSet.Date <= new DateTime(2020, 01, 01)) {
              output.WriteLine(
                  $"{resultSet.Date.ToString("yyyy/MM/dd")}: " +
                  $"{resultSet.LibYear.Total.ToString("F3")}"
              );
          }
      }

      Approvals.Verify(output);
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

      var output = new StringWriter();

      foreach (var resultSet in results)
      {
        output.WriteLine($"{resultSet.Date.ToString("yyyy/MM/dd")}: " +
                         $"{resultSet.LibYear.Total.ToString("F3")}");
      }

      var expected = new StringWriter();
      expected.WriteLine("2020/01/01: 6.126");

      Assert.Equal(expected.ToString(), output.ToString());
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

      var output = new StringWriter();

      foreach (var resultSet in results)
      {
        output.WriteLine($"{resultSet.Date.ToString("yyyy/MM/dd")}: " +
                         $"{resultSet.LibYear.Total.ToString("F3")}");
      }

      var expected = new StringWriter();
      expected.WriteLine("2020/01/01: 3.214");

      Assert.Equal(expected.ToString(), output.ToString());
    }
  }
}
