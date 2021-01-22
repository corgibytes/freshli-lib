using System;
using System.Text.RegularExpressions;
using Freshli.Exceptions;

namespace Freshli {
  /*
    SemVerVersionInfo assumes dependency versions follows the standards set
    forth by https://semver.org/.
*/
  public class SemVerVersionInfo : IVersionInfo {
    public string Version { get; private set; }
    public long? Major { get; private set; }
    public long? Minor { get; private set; }
    public long? Patch { get; private set; }
    public DateTime DatePublished { get; set; }

    public string PreRelease {
      get => _preRelease;
      set {
        ParsePreRelease(value);
        _preRelease = value;
      }
    }

    public bool IsPreRelease { get; set; }
    public string PreReleaseLabel { get; private set; }
    public long? PreReleaseIncrement { get; private set; }
    public string BuildMetadata { get; private set; }

    private readonly Regex _versionExpression = new Regex(
      @"^v?V?(\d+)[\._]?(\d+)?[\._]?(\d+)?" +
      @"(?:-?[\._]?((?:\d+|\d*[a-zA-Z-][0-9a-zA-Z-]*)" +
      @"(?:[\._](?:\d+|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?" +
      @"(?:\+([0-9a-zA-Z-]+(?:[\._][0-9a-zA-Z-]+)*))?$"
    );

    private readonly Regex _preReleaseExpression = new Regex(
      pattern: @"([a-zA-Z-]+)\.?(\d*)"
    );

    private string _preRelease;

    public SemVerVersionInfo(string version, DateTime? datePublished = null) {
      Version = version;
      if (datePublished.HasValue) { DatePublished = datePublished.Value; }

      ParseVersion();
    }

    // returns the following:
    //  -1 = less than comparer
    //  0 = equal
    //  1 = greater than comparer
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

    private void ParseVersion(
      SkippableVersionComponent? componentToSkip = null
    ) {

      if (!_versionExpression.IsMatch(Version)) {
        throw new VersionParseException(Version);
      }

      var match = _versionExpression.Match(Version);
      Major = ConvertToNullableNumber(match.Groups[1].Value);

      if (componentToSkip.HasValue) {
        int start, length;
        ProcessSkippedComponent(componentToSkip, match, out start, out length);

        Version = Version.Remove(start, length);
      }

      match = _versionExpression.Match(Version);

      Minor = ConvertToNullableNumber(match.Groups[2].Value);
      Patch = ConvertToNullableNumber(match.Groups[3].Value);
      PreRelease = ConvertToNullableString(match.Groups[4].Value);
      BuildMetadata = ConvertToNullableString(match.Groups[5].Value);
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

    private void ParsePreRelease(string value) {
      if (!String.IsNullOrEmpty(value)) {
        IsPreRelease = true;
        var match = _preReleaseExpression.Match(value);
        PreReleaseLabel = match.Groups[1].Value;
        var incrementValue = match.Groups[2].Value;
        if (!string.IsNullOrWhiteSpace(incrementValue)) {
          PreReleaseIncrement = Convert.ToInt64(incrementValue);
        }
      } else {
        IsPreRelease = false;
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

    private void ProcessSkippedComponent(
      SkippableVersionComponent? componentToSkip,
      Match match,
      out int start,
      out int length
    ) {
      start = 0;
      length = 0;

      switch(componentToSkip.Value)
      {
        case SkippableVersionComponent.Minor:
          if (match.Groups[2].Success) {
            start = match.Groups[2].Index - 1;
            length = match.Groups[2].Length + 1;
            if (match.Groups[3].Success) {
              length += match.Groups[3].Length + 1;
            }
          }
          break;
        case SkippableVersionComponent.Patch:
          if (match.Groups[3].Success) {
            start = match.Groups[3].Index - 1;
            length = match.Groups[3].Length + 1;
          }
          break;
        case SkippableVersionComponent.PreRelease:
          if (match.Groups[4].Success) {
            start = match.Groups[4].Index;
            length = match.Groups[4].Length;
            if (start > 0 && Version[start - 1] == '-') {
              start--;
              length++;
            }
          }
          break;
        case SkippableVersionComponent.BuildMetadata:
          if (match.Groups[5].Success) {
            start = match.Groups[5].Index - 1;
            length = match.Groups[5].Length + 1;
          }
          break;
        default: break;
      }
    }

    private long? ConvertToNullableNumber(string value) {
      if (string.IsNullOrWhiteSpace(value)) { return null; }

      return Convert.ToInt64(value);
    }

    private string ConvertToNullableString(string value) {
      if (string.IsNullOrWhiteSpace(value)) { return null; }

      return value;
    }
  }
}
