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

    public bool IsPostRelease => PostRelease != null;

    public int? PostReleaseSuffixType { get; set; }

    public PythonVersionPart PostRelease { get; set; }

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
        var comparisons = new List<Func<PythonVersionInfo, int>>() {
          CompareEpoch,
          CompareReleaseParts,
          CompareReleaseSuffixType,
          CompareIfDevelopmentReleaseSuffixType,
          CompareIfPreReleaseSuffixType,
          CompareIfPostReleaseSuffixType
        };

        return ProcessComparisons(comparisons, otherVersionInfo);
      } catch (Exception e) {
        throw new VersionComparisonException(
          Version,
          otherVersionInfo.Version,
          e
        );
      }
    }

    private static int ProcessComparisons(
      List<Func<PythonVersionInfo, int>> comparisons,
      PythonVersionInfo otherVersionInfo
    ) {
      var result = 0;

      foreach (var comparison in comparisons) {
        result = comparison(otherVersionInfo);
        if (result != 0) {
          return result;
        }
      }

      return result;
    }

    private int CompareEpoch(PythonVersionInfo otherVersionInfo) {
      int result = 0;
      if (Epoch.HasValue && otherVersionInfo.Epoch.HasValue) {
        result = Epoch.Value.CompareTo(otherVersionInfo.Epoch.Value);
      }

      return result;
    }

    private int CompareReleaseParts(PythonVersionInfo otherVersionInfo) {
      int result = 0;

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

      return result;
    }

    private int CompareReleaseSuffixType(PythonVersionInfo otherVersionInfo) {
      int result;
      result = VersionHelper.CompareNumericValues(
        ReleaseSuffixType,
        otherVersionInfo.ReleaseSuffixType
      );
      return result;
    }

    private int CompareIfDevelopmentReleaseSuffixType(
      PythonVersionInfo otherVersionInfo
    ) {
      int result = 0;
      if (ReleaseSuffixType == (int) SuffixType.Development) {
        result = VersionHelper.CompareNumericValues(
          DevelopmentReleaseIncrement,
          otherVersionInfo.DevelopmentReleaseIncrement
        );
      }

      return result;
    }

    private int CompareIfPreReleaseSuffixType(
      PythonVersionInfo otherVersionInfo
    ) {
      int result = 0;
      if (ReleaseSuffixType == (int) SuffixType.Pre) {
        result = ComparePreReleaseVersions(otherVersionInfo);
      }

      return result;
    }

    private int CompareIfPostReleaseSuffixType(
      PythonVersionInfo otherVersionInfo
    ) {
      int result = 0;
      if (ReleaseSuffixType == (int) SuffixType.Post) {
        result = ComparePostReleaseVersions(otherVersionInfo);
      }

      return result;
    }

    private int ComparePreReleaseVersions(PythonVersionInfo otherVersionInfo) {
      var comparisons = new List<Func<PythonVersionInfo, int>>() {
        ComparePreReleaseLabel,
        ComparePreReleaseIncrement,
        ComparePreReleaseSuffixType,
        CompareDevelopmentReleaseIncrement,
        ComparePostReleaseIncrementIfPostSuffix
      };

      return ProcessComparisons(comparisons, otherVersionInfo);
    }

    private int ComparePostReleaseIncrementIfPostSuffix(
      PythonVersionInfo otherVersionInfo
    ) {
      int result = 0;
      if (PreReleaseSuffixType == (int) SuffixType.Post) {
        result = VersionHelper.CompareNumericValues(
          PostRelease.Increment,
          otherVersionInfo.PostRelease.Increment
        );
        if (result == 0) {
          result = ComparePostReleaseVersions(otherVersionInfo);
        }
      }

      return result;
    }

    private int CompareDevelopmentReleaseIncrement(
      PythonVersionInfo otherVersionInfo
    ) {
      int result = 0;
      if (PreReleaseSuffixType == (int) SuffixType.Development) {
        result = VersionHelper.CompareNumericValues(
          DevelopmentReleaseIncrement,
          otherVersionInfo.DevelopmentReleaseIncrement
        );
      }

      return result;
    }

    private int ComparePreReleaseSuffixType(
      PythonVersionInfo otherVersionInfo
    ) {
      int result;
      result = VersionHelper.CompareNumericValues(
        PreReleaseSuffixType,
        otherVersionInfo.PreReleaseSuffixType
      );
      return result;
    }

    private int ComparePreReleaseIncrement(PythonVersionInfo otherVersionInfo) {
      int result;
      result = VersionHelper.CompareNumericValues(
        PreReleaseIncrement,
        otherVersionInfo.PreReleaseIncrement
      );
      return result;
    }

    private int ComparePreReleaseLabel(PythonVersionInfo otherVersionInfo) {
      var result = string.Compare(
        PreReleaseLabel,
        otherVersionInfo.PreReleaseLabel,
        StringComparison.InvariantCulture
      );
      return result;
    }

    private int ComparePostReleaseVersions(PythonVersionInfo otherVersionInfo) {
      var comparisons = new List<Func<PythonVersionInfo, int>>() {
        ComparePostReleaseIncrement,
        ComparePostReleaseSuffixType,
        CompareIfDevelopmentPostReleaseSuffixType,
        ComparePostReleaseIncrement
      };

      return ProcessComparisons(comparisons, otherVersionInfo);
    }

    private int CompareIfDevelopmentPostReleaseSuffixType(
      PythonVersionInfo otherVersionInfo
    ) {
      int result = 0;
      if (PostReleaseSuffixType == (int) SuffixType.Development) {
        result = VersionHelper.CompareNumericValues(
          DevelopmentReleaseIncrement,
          otherVersionInfo.DevelopmentReleaseIncrement
        );
      }

      return result;
    }

    private int ComparePostReleaseSuffixType(
      PythonVersionInfo otherVersionInfo
    ) {
      int result;
      result = VersionHelper.CompareNumericValues(
        PostReleaseSuffixType,
        otherVersionInfo.PostReleaseSuffixType
      );
      return result;
    }

    private int ComparePostReleaseIncrement(
      PythonVersionInfo otherVersionInfo
    ) {
      var result = VersionHelper.CompareNumericValues(
        PostRelease.Increment,
        otherVersionInfo.PostRelease.Increment
      );
      return result;
    }

    private void ParseVersion() {
      ResetValuesToDefaults();

      if (_versionExpression.IsMatch(_version)) {
        var match = _versionExpression.Match(_version);

        Epoch = Conversions.SafeConvertToInt64(match.Groups[2].Value, 0);
        Release = Conversions.SafeExtractString(match.Groups[3].Value);
        ReleaseParts.AddRange(
          Conversions.SafeSplitIntoLongs(match.Groups[3].Value, '.')
        );

        PreReleaseLabel = Conversions.SafeToLower(match.Groups[6].Value);
        PreReleaseIncrement =
          Conversions.SafeConvertToInt64(match.Groups[7].Value, null);
        IsPreRelease = Conversions.HasValue(PreReleaseIncrement);

        PostRelease = BuildPostRelease(
          match.Groups[9].Value,
          match.Groups[10].Value
        );

        DevelopmentReleaseIncrement =
          Conversions.SafeConvertToInt64(match.Groups[13].Value, null);
        IsDevelopmentRelease =
          Conversions.HasValue(DevelopmentReleaseIncrement);
      } else {
        throw new VersionParseException(_version);
      }
    }

    private PythonVersionPart BuildPostRelease(string label, string increment) {
      var convertedLabel = Conversions.SafeToLower(label);
      var convertedIncrement = Conversions.SafeConvertToInt64(increment, null);
      if (convertedIncrement.HasValue) {
        return new PythonVersionPart {
          Label = convertedLabel,
          Increment = convertedIncrement
        };
      }

      return null;
    }

    private void ResetValuesToDefaults() {
      Epoch = 0;
      Release = null;
      ReleaseParts = new List<long>();
      PreReleaseLabel = null;
      PreReleaseIncrement = null;
      PostRelease = null;
      DevelopmentReleaseIncrement = null;
    }

    private void SetSuffixTypes() {
      ReleaseSuffixType = null;
      PreReleaseSuffixType = null;
      PostReleaseSuffixType = null;

      if (!IsDevelopmentRelease && !IsPreRelease && !IsPostRelease) {
        ReleaseSuffixType = (int) SuffixType.NoSuffix;
      } else if (IsPreRelease) {
        ReleaseSuffixType = (int) SuffixType.Pre;
        SetPreReleaseSuffixType();
      } else if (IsDevelopmentRelease && !IsPreRelease && !IsPostRelease) {
        ReleaseSuffixType = (int) SuffixType.Development;
      } else if (IsPostRelease && !IsPreRelease) {
        ReleaseSuffixType = (int) SuffixType.Post;
        SetPostReleaseSuffixType();
      }
    }

    private void SetPreReleaseSuffixType() {
      if (!IsDevelopmentRelease && !IsPostRelease) {
        PreReleaseSuffixType = (int) SuffixType.NoSuffix;
      } else if (IsDevelopmentRelease && !IsPostRelease) {
        PreReleaseSuffixType = (int) SuffixType.Development;
      } else if (IsPostRelease) {
        PreReleaseSuffixType = (int) SuffixType.Post;
        SetPostReleaseSuffixType();
      }
    }

    private void SetPostReleaseSuffixType() {
      PostReleaseSuffixType = IsDevelopmentRelease ?
        (int) SuffixType.Development :
        (int) SuffixType.NoSuffix;
    }

    public void RemovePreReleaseMetadata() {
      PreReleaseLabel = null;
      PreReleaseIncrement = null;
      PreReleaseSuffixType = null;
      IsPreRelease = false;
    }

    public void RemovePostReleaseMetadata() {
      PostRelease = null;
      PostReleaseSuffixType = null;
    }

    public void RemoveDevelopmentReleaseMetadata() {
      DevelopmentReleaseIncrement = null;
      IsDevelopmentRelease = false;
    }

    public void RemoveLastReleaseIncrement() {
      if (Release.Contains(".")) {
        Version = Release = Release.Remove(
          Release.LastIndexOf(".", StringComparison.Ordinal)
        );
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
