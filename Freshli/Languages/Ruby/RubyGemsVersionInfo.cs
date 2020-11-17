using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Freshli.Exceptions;
using Freshli.Util;

namespace Freshli.Languages.Ruby {
  public class RubyGemsVersionInfo : IVersionInfo {
  /*
  *  RubyGemsVersionInfo assumes gem versions follows the standards set forth
  *  by
  *  https://ruby-doc.org/stdlib-2.5.0/libdoc/rubygems/rdoc/Gem/Version.html.
  */

    private string _version;

    public string Version {
      get => _version;
      set {
        _version = value;
        ParseVersion();
      }
    }

    public DateTime DatePublished { get; set; }

    public bool IsPreRelease { get; set; }

    public List<string> VersionParts { get; private set; }

    public RubyGemsVersionInfo() { }

    public RubyGemsVersionInfo(string version, DateTime datePublished) {
      Version = version;
      DatePublished = datePublished;
    }

    public int CompareTo(object other) {
      if (!(other is RubyGemsVersionInfo otherVersionInfo)) {
        throw new ArgumentException();
      }
      var result = 0;

      var versionParts = VersionParts;
      var otherVersionParts = otherVersionInfo.VersionParts;

      var countDifference =
        VersionParts.Count - otherVersionInfo.VersionParts.Count;
      for (var i = 0; i < Math.Abs(countDifference); i++) {
        if (countDifference < 0) {
          versionParts.Add("0");
        } else {
          otherVersionParts.Add("0");
        }
      }

      for (var i = 0; i < VersionParts.Count; i++) {
        result = CompareVersionParts(
          VersionParts[i],
          otherVersionInfo.VersionParts[i]
        );

        if (result != 0) {
          break;
        }
      }

      return result;
    }

    private void ParseVersion() {
      try {
        VersionParts = new List<string>();
        foreach (var versionPart in _version.Split('.')) {
          AddVersionPart(versionPart);
        }

        IsPreRelease = Regex.IsMatch(_version, @"[a-zA-Z]");
      } catch {
        throw new VersionParseException(_version);
      }
    }

    private void AddVersionPart(string part) {
      if (IsOnlyAlpha(part) || IsOnlyNumeric(part)) {
        VersionParts.Add(part);
      } else {
        var nextVersionPart = "";

        var charArr = part.ToCharArray();
        for (var i = 0; i <= charArr.Length - 1; i++) {
          nextVersionPart += charArr[i].ToString();
          if (i != charArr.Length - 1 &&
            CharacterTypesMatch(charArr[i], charArr[i + 1])) {
            continue;
          }
          VersionParts.Add(nextVersionPart);
          nextVersionPart = "";
        }
      }
    }

    private static bool IsOnlyAlpha(string s) {
      return Regex.IsMatch(s, @"^[a-zA-Z]*$");
    }

    private static bool IsOnlyAlpha(char c) {
      return IsOnlyAlpha(c.ToString());
    }

    private static bool IsOnlyNumeric(string s) {
      return Regex.IsMatch(s, @"^[\d]*$");
    }

    private static bool IsOnlyNumeric(char c) {
      return IsOnlyNumeric(c.ToString());
    }

    private static bool CharacterTypesMatch(char c1, char c2) {
      return (IsOnlyAlpha(c1) && IsOnlyAlpha(c2)) ||
        (IsOnlyNumeric(c1) && IsOnlyNumeric(c2));
    }

    private static int CompareVersionParts(string part1, string part2) {
      if (IsOnlyNumeric(part1) && IsOnlyNumeric(part2)) {
        return VersionHelper.CompareNumericValues(Convert.ToInt64(part1),
          Convert.ToInt64(part2));
      }

      if (IsOnlyAlpha(part1) && IsOnlyAlpha(part2)) {
        return String.Compare(part1, part2, StringComparison.Ordinal);
      }

      if (IsOnlyNumeric(part1)) {
        return 1;
      }

      return -1;
    }

    public override string ToString() {
      return
        $"{nameof(Version)}: {Version}, " +
        $"{nameof(DatePublished)}: {DatePublished:d}";
    }
  }
}
