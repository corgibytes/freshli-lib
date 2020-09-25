using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Freshli.Exceptions;
using Freshli.Util;

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

    public enum SuffixType {
      Development,
      Pre,
      NoSuffix,
      Post
    }

    private string _version;

    public string Version {
      get => _version;
      set {
        _version = value;
        ParseVersion();
        SetSuffixTypes();
      }
    }

    public long? Epoch { get; private set; }

    public string Release { get; private set; }
    public List<long> ReleaseParts { get; private set; }
    public int? ReleaseSuffixType { get; set; }

    public long? DevelopmentReleaseIncrement { get; private set; }
    public bool IsDevelopmentRelease { get; set; }

    public string PreReleaseLabel { get; private set; }
    public long? PreReleaseIncrement { get; private set; }
    public bool IsPreRelease { get; set; }
    public int? PreReleaseSuffixType { get; set; }

    public string PostReleaseLabel { get; private set; }
    public long? PostReleaseIncrement { get; private set; }
    public bool IsPostRelease { get; set; }
    public int? PostReleaseSuffixType { get; set; }

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
          result = VersionHelper.CompareNumericValues(
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

        result = VersionHelper.CompareNumericValues(
          ReleaseSuffixType,
          otherVersionInfo.ReleaseSuffixType
        );

        if (result != 0) {
          return result;
        }

        if (ReleaseSuffixType == (int) SuffixType.Development) {
          return VersionHelper.CompareNumericValues(
            DevelopmentReleaseIncrement,
            otherVersionInfo.DevelopmentReleaseIncrement
          );
        }

        if (ReleaseSuffixType == (int) SuffixType.Pre) {
          result = ComparePreReleaseVersions(otherVersionInfo);
        }
        if (result != 0) {
          return result;
        }

        if (ReleaseSuffixType == (int) SuffixType.Post) {
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

      result = VersionHelper.CompareNumericValues(
        PreReleaseIncrement,
        otherVersionInfo.PreReleaseIncrement
      );
      if (result != 0) {
        return result;
      }

      result = VersionHelper.CompareNumericValues(
        PreReleaseSuffixType,
        otherVersionInfo.PreReleaseSuffixType
      );
      if (result != 0) {
        return result;
      }

      if (PreReleaseSuffixType == (int) SuffixType.Development) {
        result = VersionHelper.CompareNumericValues(
          DevelopmentReleaseIncrement,
          otherVersionInfo.DevelopmentReleaseIncrement
        );
      } else if (PreReleaseSuffixType == (int) SuffixType.Post) {
        result = VersionHelper.CompareNumericValues(
          PostReleaseIncrement,
          otherVersionInfo.PostReleaseIncrement
        );
        if (result == 0) {
          result = ComparePostReleaseVersions(otherVersionInfo);
        }
      } else {
        result = VersionHelper.CompareNumericValues(
          PreReleaseIncrement,
          otherVersionInfo.PreReleaseIncrement
        );
      }

      return result;
    }

    private int ComparePostReleaseVersions(PythonVersionInfo otherVersionInfo) {

      var result = VersionHelper.CompareNumericValues(
        PostReleaseIncrement,
        otherVersionInfo.PostReleaseIncrement
      );
      if (result != 0) {
        return result;
      }

      result = VersionHelper.CompareNumericValues(
        PostReleaseSuffixType,
        otherVersionInfo.PostReleaseSuffixType
      );
      if (result != 0) {
        return result;
      }

      if (PostReleaseSuffixType == (int) SuffixType.Development) {
        result = VersionHelper.CompareNumericValues(
          DevelopmentReleaseIncrement,
          otherVersionInfo.DevelopmentReleaseIncrement
        );
      } else {
        result = VersionHelper.CompareNumericValues(
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
          DevelopmentReleaseIncrement =
            Convert.ToInt64(developmentReleaseValue);
          IsDevelopmentRelease = true;
        }
      } else {
        throw new VersionParseException(_version);
      }
    }

    private void SetSuffixTypes() {

      ReleaseSuffixType = null;
      PreReleaseSuffixType = null;
      PostReleaseSuffixType = null;

      if (!IsDevelopmentRelease && !IsPreRelease && !IsPostRelease) {
        ReleaseSuffixType = (int) SuffixType.NoSuffix;
      }
      else if (IsPreRelease) {
        ReleaseSuffixType = (int) SuffixType.Pre;
        SetPreReleaseSuffixType();
      }
      else if (IsDevelopmentRelease && !IsPreRelease && !IsPostRelease) {
        ReleaseSuffixType = (int) SuffixType.Development;
      }
      else if (IsPostRelease && !IsPreRelease) {
        ReleaseSuffixType = (int) SuffixType.Post;
        SetPostReleaseSuffixType();
      }
    }

    private void SetPreReleaseSuffixType() {
      if (!IsDevelopmentRelease && !IsPostRelease) {
        PreReleaseSuffixType = (int) SuffixType.NoSuffix;
      }
      else if (IsDevelopmentRelease && !IsPostRelease) {
        PreReleaseSuffixType = (int) SuffixType.Development;
      }
      else if (IsPostRelease) {
        PreReleaseSuffixType = (int) SuffixType.Post;
        SetPostReleaseSuffixType();
      }
    }

    private void SetPostReleaseSuffixType() {
      PostReleaseSuffixType = IsDevelopmentRelease ?
          (int) SuffixType.Development : (int) SuffixType.NoSuffix;
    }

    public void RemovePreReleaseMetadata() {
      PreReleaseLabel = null;
      PreReleaseIncrement = null;
      PreReleaseSuffixType = null;
      IsPreRelease = false;
    }

    public void RemovePostReleaseMetadata() {
      PostReleaseLabel = null;
      PostReleaseIncrement = null;
      PostReleaseSuffixType = null;
      IsPostRelease = false;
    }

    public void RemoveDevelopmentReleaseMetadata() {
      DevelopmentReleaseIncrement = null;
      IsDevelopmentRelease = false;
    }

    public void RemoveLastReleaseIncrement() {
      if (Release.Contains(".")) {
        Version = Release = Release.Remove(Release.LastIndexOf
          (".", StringComparison.Ordinal));
        ReleaseSuffixType = (int) SuffixType.NoSuffix;
      }
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
