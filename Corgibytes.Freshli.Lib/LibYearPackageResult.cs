using System;

namespace Corgibytes.Freshli.Lib
{
    // TODO: Explore converting this `class` into a `record`
    public class LibYearPackageResult
    {
        public string Name { get; }
        public string Version { get; }
        public DateTimeOffset PublishedAt { get; }
        public string LatestVersion { get; }
        public DateTimeOffset LatestPublishedAt { get; }
        public double Value { get; }
        public bool UpgradeAvailable { get; set; }
        public bool Skipped { get; }

        public LibYearPackageResult(
          string name,
          string version,
          DateTimeOffset publishedAt,
          string latestVersion,
          DateTimeOffset latestPublishedAt,
          double value,
          bool upgradeAvailable,
          bool skipped
        )
        {
            Name = name;
            Version = version;
            PublishedAt = publishedAt;
            LatestVersion = latestVersion;
            LatestPublishedAt = latestPublishedAt;
            Value = value;
            UpgradeAvailable = upgradeAvailable;
            Skipped = skipped;
        }

        public LibYearPackageResult(
          string name,
          IVersionInfo version,
          IVersionInfo latestVersion,
          double value,
          bool upgradeAvailable,
          bool skipped
        )
        {
            Name = name;
            Version = version.Version;
            PublishedAt = version.DatePublished;
            LatestVersion = latestVersion.Version;
            LatestPublishedAt = latestVersion.DatePublished;
            Value = value;
            UpgradeAvailable = upgradeAvailable;
            Skipped = skipped;
        }

        public override string ToString()
        {
            return
              $"{{ Name: \"{Name}\", " +
              $"RepoVersion: \"{Version}\", " +
              $"RepoVersionPublishedAt: {PublishedAt:O}, " +
              $"LatestVersion: \"{LatestVersion}\", " +
              $"LatestPublishedAt: {LatestPublishedAt:O}, " +
              $"UpgradeAvailable: {UpgradeAvailable}, " +
              $"Value: {Value} }}";
        }
    }
}
