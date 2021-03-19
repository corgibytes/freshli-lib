using System;
using System.Collections.Generic;
using System.Linq;
using ApprovalTests;
using Freshli.Languages.Ruby;
using Freshli.Util;
using Xunit;

namespace Freshli.Test.Integration.Languages.Ruby {
  public class RubyGemsRepositoryTest {

    [Fact]
    public void VersionInfoCorrectlyCreatesVersion() {
      var repository = new RubyGemsRepository();
      var versionInfo = repository.VersionInfo("tzinfo", "1.2.7");
      var expectedDate =
        new DateTimeOffset(2020, 04, 02, 21, 42, 11, TimeSpan.Zero);

      Assert.Equal("1.2.7", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void VersionInfoCorrectlyCreatesPreReleaseVersion() {
      var repository = new RubyGemsRepository();
      var versionInfo = repository.VersionInfo("git", "1.6.0.pre1");
      var expectedDate =
        new DateTimeOffset(2020, 01, 20, 20, 50, 43, TimeSpan.Zero);

      Assert.Equal("1.6.0.pre1", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void LatestAsOfCorrectlyFindsLatestVersion() {
      var repository = new RubyGemsRepository();
      var targetDate =
        new DateTimeOffset(2020, 02, 01, 00, 00, 00, TimeSpan.Zero);
      var versionInfo = repository.Latest("git", targetDate, false);
      var expectedDate =
        new DateTimeOffset(2018, 08, 10, 07, 58, 25, TimeSpan.Zero);

      Assert.Equal("1.5.0", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void
      LatestAsOfCorrectlyFindsLatestPreReleaseVersion() {
      var repository = new RubyGemsRepository();
      var targetDate =
        new DateTimeOffset(2020, 02, 01, 00, 00, 00, TimeSpan.Zero);
      var versionInfo = repository.Latest("git", targetDate, true);
      var expectedDate =
        new DateTimeOffset(2020, 01, 20, 20, 50, 43, TimeSpan.Zero);

      Assert.Equal("1.6.0.pre1", versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    [Fact]
    public void VersionsBetweenFindsVersionsReleasedBeforeTargetDate() {
    var repository = new RubyGemsRepository();
    var targetDate =
      new DateTimeOffset(2014, 04, 01, 00, 00, 00, TimeSpan.Zero);
    var earlierVersion = new RubyGemsVersionInfo {Version = "0.3.38"};
    var laterVersion = new RubyGemsVersionInfo {Version = "1.1.0"};

    var versions = repository.VersionsBetween("tzinfo", targetDate,
      earlierVersion, laterVersion, includePreReleases: true);

    Assert.Equal(3, versions.Count);
    }

    [Fact]
    public void VersionsBetweenCorrectlyFindsVersions() {
      var repository = new RubyGemsRepository();
      var targetDate =
        new DateTimeOffset(2020, 09, 01, 00, 00, 00, TimeSpan.Zero);
      var earlierVersion = new RubyGemsVersionInfo {Version = "3.11.0"};
      var laterVersion = new RubyGemsVersionInfo {Version = "3.13.0"};

      var versions = repository.VersionsBetween(name: "google-protobuf",
        targetDate, earlierVersion, laterVersion, includePreReleases: false);

      Assert.Equal(8, versions.Count);
    }


    [Fact]
    public void VersionsBetweenCorrectlyFindsVersionsWithPreReleases() {
      var repository = new RubyGemsRepository();
      var targetDate =
        new DateTimeOffset(2020, 09, 01, 00, 00, 00, TimeSpan.Zero);
      var earlierVersion = new RubyGemsVersionInfo {Version = "3.11.0"};
      var laterVersion = new RubyGemsVersionInfo {Version = "3.13.0"};

      var versions = repository.VersionsBetween(name: "google-protobuf",
        targetDate, earlierVersion, laterVersion, includePreReleases: true);

      Assert.Equal(11, versions.Count);
    }

    [Fact]
    public void ProperlyHandlesRateLimits() {
      var repository = new RubyGemsRepository();
      var targetDate =
        new DateTimeOffset(2021, 03, 10, 00, 00, 00, TimeSpan.Zero);

      var packages = new[] {
        "actioncable",
        "actionmailer",
        "actionpack",
        "actionview",
        "active_entry",
        "activeadmin_froala_editor",
        "activeadmin_trumbowyg",
        "activecube",
        "activejob",
        "activemodel",
        "activerecord",
        "activestorage",
        "activesupport",
        "anpo",
        "asciidoctor-diagram-ditaamini",
        "asciidoctor-diagram-plantuml",
        "ast",
        "autoprefixer-rails",
        "awesome_print",
        "aws-eventstream",
        "aws-partitions",
        "aws-sdk",
        "aws-sdk-apigateway",
        "aws-sdk-core",
        "aws-sdk-dynamodb",
        "aws-sdk-ec2",
        "aws-sdk-kms",
        "aws-sdk-resources",
        "aws-sdk-s3",
        "aws-sdk-sagemaker",
        "aws-sdk-sqs",
        "aws-sdk-ssm",
        "aws-sigv4",
        "backports",
        "betterplace-content",
        "bibliothecary",
        "blazeverify",
        "buildkit",
        "bundler",
        "capybara",
        "concurrent-ruby",
        "daily_notices",
        "database_cleaner",
        "decidim-bulletin_board",
        "devise_masquerade",
        "docile",
        "excon",
        "faker",
        "faraday",
        "ffi",
        "flowcommerce",
        "fluent-plugin-elasticsearch",
        "fog-aws",
        "google-api-client",
        "googleauth",
        "govuk_publishing_components",
        "graphiti",
        "graphiti_graphql",
        "gravid",
        "gtk_paradise",
        "heartcheck-activerecord",
        "i18n",
        "jbuilder",
        "kitchen-azurerm",
        "knife-cookbook-doc",
        "listen",
        "loofah",
        "mime-types-data",
        "minitest",
        "msgpack",
        "mumuki-styles",
        "mxfinfo",
        "newrelic_rpm",
        "nio4r",
        "nokogiri",
        "nokogumbo",
        "oj",
        "omniauth",
        "opener-chained-daemon",
        "ox",
        "page-object",
        "parsecs",
        "power-bi",
        "prick",
        "pry",
        "puma",
        "pusher-push-notifications",
        "rails",
        "rails-deprecated_sanitizer",
        "railties",
        "redis-namespace",
        "reform-rails",
        "regexp_parser",
        "relaton-bib",
        "relaton-gb",
        "relaton-iec",
        "relaton-ieee",
        "relaton-ietf",
        "relaton-iho",
        "relaton-iso",
        "relaton-itu",
        "relaton-nist",
        "relaton-ogc",
        "relaton-un",
        "relaton-w3c",
        "repltalk",
        "restful_resource_bugsnag",
        "rpg_paradise",
        "rspec-mocks",
        "rspec-rails",
        "rspec-support",
        "rubocop",
        "rubocop-codeur",
        "rubygems-update",
        "salsify_rubocop",
        "schemacop",
        "sdoc",
        "sentry-rails",
        "shoulda-matchers",
        "sidekiq",
        "sidekiq-unique-jobs",
        "signet",
        "sila-ruby",
        "simplecov",
        "simplecov-sublime",
        "slow-steps",
        "spina",
        "spoom",
        "spreewald",
        "swagger-dsl",
        "takelage",
        "tanker-core",
        "terminal-table",
        "text_alignment",
        "thor",
        "timecop",
        "timers",
        "todo-jsonl",
        "turbograft",
        "unforgettable",
        "universal-track-manager",
        "vite_rails",
        "vite_rails_legacy",
        "vite_ruby",
        "webmock",
      }.ToList();

      var packagesAndResults = new Dictionary<string, List<IVersionInfo>>();

      packages.ForEach(
        package => {
          var latest = repository.Latest(
            package,
            asOf: targetDate,
            true
          );

          var allVersions = repository.VersionsBetween(
            name: package,
            targetDate,
            RubyGemsVersionInfo.MinimumVersion,
            new RubyGemsVersionInfo(latest.Version, latest.DatePublished),
            includePreReleases: true
          );

          packagesAndResults.Add(package, allVersions);
        }
      );

      Approvals.VerifyAll(packagesAndResults, (package, values) => {
          var joinedValues = String.Join(
            ", ",
            values.Select(v => v.ToString()).ToList()
          );
          return $"{package}: [ {joinedValues} ]";
        }
      );
    }
  }
}
