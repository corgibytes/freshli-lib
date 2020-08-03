using System;
using System.Text.RegularExpressions;
using Freshli.Exceptions;
using NLog;

namespace Freshli {
  /*
    SemVerVersionInfo assumes dependency versions follows the standards set
    forth by https://semver.org/.
*/
  public class SemVerVersionInfo : IVersionInfo {
    private string _version;

    public string Version {
      get => _version;
      set {
        _version = value;
        ParseVersion();
      }
    }

    public long? Major { get; private set; }
    public long? Minor { get; private set; }
    public long? Patch { get; private set; }

    public string PreRelease {
      get => _preRelease;
      set {
        ParsePreRelease(value);
        _preRelease = value;
      }
    }

    public string PreReleaseLabel { get; private set; }
    public long? PreReleaseIncrement { get; private set; }
    public string BuildMetadata { get; private set; }

    private readonly Regex _versionExpression = new Regex(
      @"^v?(\d+)[\._]?(\d+)?[\._]?(\d+)?" +
      @"(?:-?[\._]?((?:\d+|\d*[a-zA-Z-][0-9a-zA-Z-]*)" +
      @"(?:[\._](?:\d+|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?" +
      @"(?:\+([0-9a-zA-Z-]+(?:[\._][0-9a-zA-Z-]+)*))?$"
    );

    private readonly Regex _preReleaseExpression = new Regex(
      pattern: @"([a-zA-Z-]+)\.?(\d*)"
    );

    private string _preRelease;

    private void ParsePreRelease(string value) {
      if (!String.IsNullOrEmpty(value)) {
        var match = _preReleaseExpression.Match(value);
        PreReleaseLabel = match.Groups[1].Value;
        var incrementValue = match.Groups[2].Value;
        if (!string.IsNullOrWhiteSpace(incrementValue)) {
          PreReleaseIncrement = Convert.ToInt64(incrementValue);
        }
      } else {
        PreReleaseLabel = null;
        PreReleaseIncrement = null;
      }
    }

    private enum SkippableVersionComponent {
      Minor,
      Patch,
      PreRelease,
      BuildMetadata
    }

    private void ParseVersion(
      SkippableVersionComponent? componentToSkip = null
    ) {
      Major = null;
      Minor = null;
      Patch = null;
      PreRelease = null;
      BuildMetadata = null;

      if (_versionExpression.IsMatch(_version)) {
        var match = _versionExpression.Match(_version);

        var majorValue = match.Groups[1].Value;
        if (!string.IsNullOrWhiteSpace(majorValue)) {
          Major = Convert.ToInt64(majorValue);
        }

        if (componentToSkip.HasValue) {
          var start = 0;
          var length = 0;

          if (componentToSkip.Value == SkippableVersionComponent.Minor) {
            if (match.Groups[2].Success) {
              start = match.Groups[2].Index - 1;
              length = match.Groups[2].Length + 1;
              if (match.Groups[3].Success) {
                length += match.Groups[3].Length + 1;
              }
            }
          } else if (componentToSkip.Value == SkippableVersionComponent.Patch) {
            if (match.Groups[3].Success) {
              start = match.Groups[3].Index - 1;
              length = match.Groups[3].Length + 1;
            }
          } else if (componentToSkip == SkippableVersionComponent.PreRelease) {
            if (match.Groups[4].Success) {
              start = match.Groups[4].Index;
              length = match.Groups[4].Length;
              if (start > 0 && _version[start - 1] == '-') {
                start--;
                length++;
              }
            }
          } else if (componentToSkip ==
            SkippableVersionComponent.BuildMetadata) {
            if (match.Groups[5].Success) {
              start = match.Groups[5].Index - 1;
              length = match.Groups[5].Length + 1;
            }
          }

          _version = _version.Remove(start, length);
        }

        match = _versionExpression.Match(_version);

        var minorValue = match.Groups[2].Value;
        Minor = null;
        if (!string.IsNullOrWhiteSpace(minorValue)) {
          Minor = Convert.ToInt64(minorValue);
        }

        var patchValue = match.Groups[3].Value;
        Patch = null;
        if (!string.IsNullOrWhiteSpace(patchValue)) {
          Patch = Convert.ToInt64(patchValue);
        }

        var preReleaseValue = match.Groups[4].Value;
        PreRelease = null;
        if (!string.IsNullOrWhiteSpace(preReleaseValue)) {
          PreRelease = preReleaseValue;
        }

        var buildMetadataValue = match.Groups[5].Value;
        BuildMetadata = null;
        if (!string.IsNullOrWhiteSpace(buildMetadataValue)) {
          BuildMetadata = buildMetadataValue;
        }
      } else {
        throw new VersionParseException(_version);
      }
    }

    public void RemoveBuildMetadata() {
      ParseVersion(SkippableVersionComponent.BuildMetadata);
    }

    public void RemovePreRelease() {
      ParseVersion(SkippableVersionComponent.PreRelease);
    }

    public void RemovePatch() {
      ParseVersion(SkippableVersionComponent.Patch);
    }

    public void RemoveMinor() {
      ParseVersion(SkippableVersionComponent.Minor);
    }

    public DateTime DatePublished { get; set; }

    public SemVerVersionInfo() { }

    public SemVerVersionInfo(string version, DateTime datePublished) {
      Version = version;
      DatePublished = datePublished;
    }

    public int CompareTo(object other) {
      var otherVersionInfo = other as SemVerVersionInfo;
      if (otherVersionInfo == null) {
        throw new ArgumentException();
      }

      var result = 0;

      if (Major.HasValue && otherVersionInfo.Major.HasValue) {
        result = Major.Value.CompareTo(otherVersionInfo.Major.Value);
        if (result != 0) {
          return result;
        }
      }

      if (Minor.HasValue && otherVersionInfo.Minor.HasValue) {
        result = Minor.Value.CompareTo(otherVersionInfo.Minor.Value);
      } else if (Minor.HasValue) {
        result = 1;
        if (Minor.Value == 0) {
          result = 0;
        }
      } else if (otherVersionInfo.Minor.HasValue) {
        result = -1;
        if (otherVersionInfo.Minor.Value == 0) {
          result = 0;
        }
      }

      if (result != 0) {
        return result;
      }

      if (Patch.HasValue && otherVersionInfo.Patch.HasValue) {
        result = Patch.Value.CompareTo(otherVersionInfo.Patch.Value);
      } else if (Patch.HasValue) {
        result = 1;
        if (Patch.Value == 0) {
          result = 0;
        }
      } else if (otherVersionInfo.Patch.HasValue) {
        result = -1;
        if (otherVersionInfo.Patch.Value == 0) {
          result = 0;
        }
      }

      if (result != 0) {
        return result;
      }

      if (PreRelease != null && otherVersionInfo.PreRelease != null) {
        result = String.Compare(
          PreReleaseLabel,
          otherVersionInfo.PreReleaseLabel,
          StringComparison.Ordinal
        );

        if (result != 0) {
          return result;
        }

        if (PreReleaseIncrement.HasValue &&
          otherVersionInfo.PreReleaseIncrement.HasValue) {
          result = PreReleaseIncrement.Value.CompareTo(
            otherVersionInfo.PreReleaseIncrement.Value
          );
        } else if (PreReleaseIncrement.HasValue) {
          result = 1;
        } else if (otherVersionInfo.PreReleaseIncrement.HasValue) {
          result = -1;
        }
      } else if (PreRelease != null) {
        result = -1;
      } else if (otherVersionInfo.PreRelease != null) {
        result = 1;
      }

      if (result != 0) {
        return result;
      }

      return 0;
    }

    public override string ToString() {
      return
        $"{nameof(Major)}: {Major}, " +
        $"{nameof(Minor)}: {Minor}, " +
        $"{nameof(Patch)}: {Patch}, " +
        $"{nameof(PreRelease)}: {PreRelease}, " +
        $"{nameof(BuildMetadata)}: {BuildMetadata}, " +
        $"{nameof(DatePublished)}: {DatePublished:d}";
    }

    public string ToSimpleVersion() {
      return $"{Major}.{Minor}.{Patch}";
    }
  }
}
