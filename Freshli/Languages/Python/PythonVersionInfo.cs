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

    private string _version;

    public string Version {
      get => _version;
      set {
        _version = value;
        ParseVersion();
      }
    }

    public long? Epoch { get; private set; }

    public string Release { get; private set; }

    public List<long> ReleaseParts { get; private set; }

    public string PreReleaseLabel { get; private set; }

    public long? PreReleaseValue { get; private set; }

    public string PostReleaseLabel { get; private set; }

    public long? PostReleaseValue { get; private set; }

    public long? DevelopmentReleaseValue { get; private set; }

    public DateTime DatePublished { get; set; }

    private readonly Regex _versionExpression = new Regex(
      @"^(?i)(([0-9]+)!)?" +
      @"([0-9]+(\.[0-9]+)*)" +
      @"((a|b|rc|alpha|beta|pre|preview)([0-9]+))?" +
      @"(\.(post|rev|r)([0-9]+))?" +
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
          if (result != 0) {
            return result;
          }
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

        //TODO: Compare additional parts

        return result;
      } catch (Exception e)
      {
        throw new Exception(e.Message);
      }
    }

    private void ParseVersion() {
      Epoch = 0;
      Release = null;
      ReleaseParts = new List<long>();
      PreReleaseLabel = null;
      PreReleaseValue = null;
      PostReleaseLabel = null;
      PostReleaseValue = null;
      DevelopmentReleaseValue = null;

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
          PreReleaseValue = Convert.ToInt64(preReleaseValue);
        }

        var postReleaseLabel = match.Groups[9].Value;
        if (!string.IsNullOrWhiteSpace(postReleaseLabel)) {
          PostReleaseLabel = postReleaseLabel.ToLower();
        }
        var postReleaseValue = match.Groups[10].Value;
        if (!string.IsNullOrWhiteSpace(postReleaseValue)) {
          PostReleaseValue = Convert.ToInt64(postReleaseValue);
        }

        var developmentReleaseValue = match.Groups[13].Value;
        if (!string.IsNullOrWhiteSpace(developmentReleaseValue)) {
          DevelopmentReleaseValue = Convert.ToInt64(developmentReleaseValue);
        }
      } else {
        throw new VersionParseException(_version);
      }
    }

    //TODO: See if needed
    private bool IsFinal() {
      return
        PreReleaseLabel == null &&
        PreReleaseValue == null &&
        PostReleaseLabel == null &&
        PostReleaseValue == null &&
        DevelopmentReleaseValue == null;
    }

    //TODO: Consolidate with logic in RubyGemsVersionInfo
    private static int CompareNumericValues(long v1, long v2) {
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
