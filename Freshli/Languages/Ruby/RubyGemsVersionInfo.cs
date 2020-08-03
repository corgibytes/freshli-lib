using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Freshli.Exceptions;

namespace Freshli.Languages.Ruby {
  public class RubyGemsVersionInfo : IVersionInfo {
    /*
     * RubyGemsVersionInfo assumes gem versions follows the standards set forth
     * by
     * https://ruby-doc.org/stdlib-2.5.0/libdoc/rubygems/rdoc/Gem/Version.html.
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

    public bool IsPreRelease { get; private set; }

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

      var versionPartsCount = VersionParts.Count();
      var versionParts = VersionParts;
      var otherVersionPartsCount = otherVersionInfo.VersionParts.Count();
      var otherVersionParts = otherVersionInfo.VersionParts;

      var countDifference = versionPartsCount - otherVersionPartsCount;

      if (countDifference < 0) {
        for (var i = 0; i < Math.Abs(countDifference); i++) {
          versionParts.Add("0");
        }
      }
      else
      {
        for (var i = 0; i < Math.Abs(countDifference); i++) {
          otherVersionParts.Add("0");
        }
      }

      for (var i = 0; i < VersionParts.Count; i++) {
        result = String.Compare(VersionParts[i], otherVersionInfo.VersionParts[i], StringComparison.Ordinal);
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
