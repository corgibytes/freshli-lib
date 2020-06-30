using System;

namespace Freshli {
  public class LibYearPackageResult {
    public string Name { get; }
    public string Version { get; }
    public DateTime PublishedAt { get; }
    public double Value { get; }

    public LibYearPackageResult(
      string name,
      string version,
      DateTime publishedAt,
      double value
    ) {
      Name = name;
      Version = version;
      PublishedAt = publishedAt;
      Value = value;
    }

    public override string ToString() {
      return
        $"{{ Name: \"{Name}\", " +
        $"Version: \"{Version}\", " +
        $"PublishedAt: {PublishedAt:s}, " +
        $"Value: {Value} }}";
    }
  }
}
