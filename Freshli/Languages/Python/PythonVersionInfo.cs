using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Freshli.Exceptions;

namespace Freshli.Languages.Python {
  public class PythonVersionInfo : IVersionInfo {

    /*
     * PythonVersionInfo assumes package versions follow the standards set forth
     * by https://www.python.org/dev/peps/pep-0440:
     *
     * Epoch segment: N!
     * Release segment: N(.N)*
     * Pre-release segment: {a|b|rc}N
     * Post-release segment: .postN
     * Development release segment: .devN
     */

    //TODO: Move to enum
    public const int Development = 100;
    public const int Pre = 200;
    public const int NoSuffix = 300;
    public const int Post = 400;

    private string _version;

    public string Version {
      get => _version;
      set {
        _version = value;
        ParseVersion();
        SetSortPositions();
      }
    }

    public long? Epoch { get; private set; }

    public string Release { get; private set; }
    public List<long> ReleaseParts { get; private set; }
    public int? ReleaseSortPosition { get; set; }

    public long? DevelopmentReleaseIncrement { get; private set; }
    public bool IsDevelopmentRelease { get; set; }

    public string PreReleaseLabel { get; private set; }
    public long? PreReleaseIncrement { get; private set; }
    public bool IsPreRelease { get; set; }
    public int? PreReleaseSortPosition { get; set; }

    public string PostReleaseLabel { get; private set; }
    public long? PostReleaseIncrement { get; private set; }
    public bool IsPostRelease { get; set; }
    public int? PostReleaseSortPosition { get; set; }

    public DateTime DatePublished { get; set; }

    private readonly Regex _versionExpression = new Regex(
      @"^(?i)(([0-9]+)!)?" +
      @"([0-9]+(\.[0-9]+)*)" +
      @"((a|b|rc)([0-9]+))?" +
      @"(\.(post)([0-9]+))?" +
      @"(\.(dev)([0-9]+))?(?-i)$"
    );

    public PythonVersionInfo() { }

    public PythonVersionInfo(string version, DateTime datePublished) {
      Version = version;
      DatePublished = datePublished;
    }

    public int CompareTo(object other) {
      if (!(other is PythonVersionInfo otherVersionInfo)) {
        throw new ArgumentException();
      }

      try {
        var result = 0;

        if (Epoch.HasValue && otherVersionInfo.Epoch.HasValue) {
          result = Epoch.Value.CompareTo(otherVersionInfo.Epoch.Value);
        }
        if (result != 0) {
          return result;
        }

        var releaseParts = ReleaseParts;
        var otherReleaseParts = otherVersionInfo.ReleaseParts;
        var countDifference =
          ReleaseParts.Count - otherVersionInfo.ReleaseParts.Count;

        for (var i = 0; i < Math.Abs(countDifference); i++) {
          if (countDifference < 0) {
            releaseParts.Add(0);
          } else {
            otherReleaseParts.Add(0);
          }
        }
        for (var i = 0; i < ReleaseParts.Count; i++) {
          result = CompareNumericValues(
            ReleaseParts[i],
            otherVersionInfo.ReleaseParts[i]
          );
          if (result != 0) {
            break;
          }
        }
        if (result != 0) {
          return result;
        }

        result = CompareNumericValues(
          ReleaseSortPosition,
          otherVersionInfo.ReleaseSortPosition
        );

        if (result != 0) {
          return result;
        }

        if (ReleaseSortPosition == Development) {
          return CompareNumericValues(
            DevelopmentReleaseIncrement,
            otherVersionInfo.DevelopmentReleaseIncrement
          );
        }

        if (ReleaseSortPosition == Pre) {
          result = ComparePreReleaseVersions(otherVersionInfo);
        }
        if (result != 0) {
          return result;
        }

        if (ReleaseSortPosition == Post) {
          result = ComparePostReleaseVersions(otherVersionInfo);
        }

        return result;

      }
      catch (Exception e) {
        throw new VersionComparisonException(
          Version, otherVersionInfo.Version, e);
      }
    }

    private int ComparePreReleaseVersions(PythonVersionInfo otherVersionInfo) {

      var result = string.Compare(
        PreReleaseLabel,
        otherVersionInfo.PreReleaseLabel,
        StringComparison.InvariantCulture
      );
      if (result != 0) {
        return result;
      }

      result =  CompareNumericValues(
        PreReleaseIncrement,
        otherVersionInfo.PreReleaseIncrement
      );
      if (result != 0) {
        return result;
      }

      result = CompareNumericValues(
        PreReleaseSortPosition,
        otherVersionInfo.PreReleaseSortPosition
      );
      if (result != 0) {
        return result;
      }

      if (PreReleaseSortPosition == Development) {
        result = CompareNumericValues(
          DevelopmentReleaseIncrement,
          otherVersionInfo.DevelopmentReleaseIncrement
        );
      } else if (PreReleaseSortPosition == Post) {
        result = CompareNumericValues(
          PostReleaseIncrement,
          otherVersionInfo.PostReleaseIncrement
        );
        if (result == 0) {
          result = ComparePostReleaseVersions(otherVersionInfo);
        }
      } else {
        result = CompareNumericValues(
          PreReleaseIncrement,
          otherVersionInfo.PreReleaseIncrement
        );
      }

      return result;
    }

    private int ComparePostReleaseVersions(PythonVersionInfo otherVersionInfo) {

      var result =  CompareNumericValues(
        PostReleaseIncrement,
        otherVersionInfo.PostReleaseIncrement
      );
      if (result != 0) {
        return result;
      }

      result = CompareNumericValues(
        PostReleaseSortPosition,
        otherVersionInfo.PostReleaseSortPosition
      );
      if (result != 0) {
        return result;
      }

      if (PostReleaseSortPosition == Development) {
        result = CompareNumericValues(
          DevelopmentReleaseIncrement,
          otherVersionInfo.DevelopmentReleaseIncrement
        );
      } else {
        result = CompareNumericValues(
          PostReleaseIncrement,
          otherVersionInfo.PostReleaseIncrement
        );
      }

      return result;
    }

    private void ParseVersion() {
      Epoch = 0;
      Release = null;
      ReleaseParts = new List<long>();
      PreReleaseLabel = null;
      PreReleaseIncrement = null;
      PostReleaseLabel = null;
      PostReleaseIncrement = null;
      DevelopmentReleaseIncrement = null;

      if (_versionExpression.IsMatch(_version)) {
        var match = _versionExpression.Match(_version);

        var epoch = match.Groups[2].Value;
        if (!string.IsNullOrWhiteSpace(epoch)) {
          Epoch = Convert.ToInt64(epoch);
        }

        var release = match.Groups[3].Value;
        if (!string.IsNullOrWhiteSpace(release)) {
          Release = release;
          foreach (var part in Release.Split('.')) {
            ReleaseParts.Add(Convert.ToInt64(part));
          }
        }

        var preReleaseLabel = match.Groups[6].Value;
        if (!string.IsNullOrWhiteSpace(preReleaseLabel)) {
          PreReleaseLabel = preReleaseLabel.ToLower();
        }
        var preReleaseValue = match.Groups[7].Value;
        if (!string.IsNullOrWhiteSpace(preReleaseValue)) {
          PreReleaseIncrement = Convert.ToInt64(preReleaseValue);
          IsPreRelease = true;
        }

        var postReleaseLabel = match.Groups[9].Value;
        if (!string.IsNullOrWhiteSpace(postReleaseLabel)) {
          PostReleaseLabel = postReleaseLabel.ToLower();
        }
        var postReleaseValue = match.Groups[10].Value;
        if (!string.IsNullOrWhiteSpace(postReleaseValue)) {
          PostReleaseIncrement = Convert.ToInt64(postReleaseValue);
          IsPostRelease = true;
        }

        var developmentReleaseValue = match.Groups[13].Value;
        if (!string.IsNullOrWhiteSpace(developmentReleaseValue)) {
          DevelopmentReleaseIncrement = Convert.ToInt64(developmentReleaseValue);
          IsDevelopmentRelease = true;
        }
      } else {
        throw new VersionParseException(_version);
      }
    }

    private void SetSortPositions() {

      ReleaseSortPosition = null;
      PreReleaseSortPosition = null;
      PostReleaseSortPosition = null;

      if (!IsDevelopmentRelease && !IsPreRelease && !IsPostRelease) {
        ReleaseSortPosition = NoSuffix;
      }
      else if (IsPreRelease) {
        ReleaseSortPosition = Pre;
        SetPreReleaseSortPosition();
      }
      else if (IsDevelopmentRelease && !IsPreRelease && !IsPostRelease) {
        ReleaseSortPosition = Development;
      }
      else if (IsPostRelease && !IsPreRelease) {
        ReleaseSortPosition = Post;
        SetPostReleaseSortPosition();
      }
    }

    private void SetPreReleaseSortPosition() {
      if (!IsDevelopmentRelease && !IsPostRelease) {
        PreReleaseSortPosition = NoSuffix;
      }
      else if (IsDevelopmentRelease && !IsPostRelease) {
        PreReleaseSortPosition = Development;
      }
      else if (IsPostRelease) {
        PreReleaseSortPosition = Post;
        SetPostReleaseSortPosition();
      }
    }

    private void SetPostReleaseSortPosition() {
      PostReleaseSortPosition = IsDevelopmentRelease ? Development : NoSuffix;
    }

    //TODO: Consolidate with logic in RubyGemsVersionInfo
    private static int CompareNumericValues(long? v1, long? v2) {
      if (v1 == v2) {
        return 0;
      }
      if (v1 > v2) {
        return 1;
      }
      return -1;
    }

    public override string ToString() {
      return
        $"{nameof(Version)}: {Version}, " +
        $"{nameof(DatePublished)}: {DatePublished:d}";
    }

    public string ToSimpleVersion() {
      return $"{Version}";
    }
  }
}
