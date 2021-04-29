using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Corgibytes.Freshli.Lib.Exceptions;
using Corgibytes.Freshli.Lib.Util;

namespace Corgibytes.Freshli.Lib.Languages.Python
{
    public class PythonVersionInfo : IVersionInfo
    {
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

        public enum SuffixType
        {
            Development,
            Pre,
            NoSuffix,
            Post
        }

        private string _version;
        public string Version
        {
            get => _version;
            private set
            {
                _version = value;
                ParseVersion();
                SetSuffixTypes();
            }
        }

        public long? Epoch { get; private set; }
        public string Release { get; private set; }
        public List<long> ReleaseParts { get; private set; }

        public SuffixType ReleaseSuffixType
        {
            get
            {
                if (ReleaseSuffix != null)
                {
                    return ReleaseSuffix.Type;
                }

                return SuffixType.NoSuffix;
            }
        }

        public PythonVersionPart ReleaseSuffix { get; set; }

        public PythonVersionPart DevelopmentRelease { get; set; }
        public bool IsDevelopmentRelease => DevelopmentRelease != null;

        public PythonVersionPart PreRelease { get; set; }
        public bool IsPreRelease => PreRelease != null;
        public int? PreReleaseSuffixType { get; set; }

        public PythonVersionPart PostRelease { get; set; }
        public bool IsPostRelease => PostRelease != null;
        public int? PostReleaseSuffixType { get; set; }

        public DateTime DatePublished { get; set; }

        private readonly Regex _versionExpression = new Regex(
          @"^(?i)(([0-9]+)!)?" +
          @"([0-9]+(\.[0-9]+)*)" +
          @"((a|b|rc)([0-9]+))?" +
          @"(\.(post)([0-9]+))?" +
          @"(\.(dev)([0-9]+))?(?-i)$"
        );

        public PythonVersionInfo(string version, DateTime? datePublished = null)
        {
            Version = version;
            if (datePublished.HasValue) { DatePublished = datePublished.Value; }
        }

        public int CompareTo(object other)
        {
            if (!(other is PythonVersionInfo otherVersionInfo))
            {
                throw new ArgumentException();
            }

            var comparisons = new List<Func<PythonVersionInfo, int>>() {
        CompareEpoch,
        CompareReleaseParts,
        CompareReleaseSuffixType,
        CompareIfDevelopmentReleaseSuffixType,
        CompareIfPreReleaseSuffixType,
        CompareIfPostReleaseSuffixType
      };

            return ProcessComparisons(comparisons, otherVersionInfo);
        }

        private static int ProcessComparisons(
          List<Func<PythonVersionInfo, int>> comparisons,
          PythonVersionInfo otherVersionInfo
        )
        {
            var result = 0;

            foreach (var comparison in comparisons)
            {
                result = comparison(otherVersionInfo);
                if (result != 0)
                {
                    return result;
                }
            }

            return result;
        }

        private int CompareEpoch(PythonVersionInfo otherVersionInfo)
        {
            return Epoch.HasValue && otherVersionInfo.Epoch.HasValue ?
              Epoch.Value.CompareTo(otherVersionInfo.Epoch.Value) :
              0;
        }

        private int CompareReleaseParts(PythonVersionInfo otherVersionInfo)
        {
            int result = 0;

            var releaseParts = ReleaseParts;
            var otherReleaseParts = otherVersionInfo.ReleaseParts;
            var countDifference =
              ReleaseParts.Count - otherVersionInfo.ReleaseParts.Count;

            for (var i = 0; i < Math.Abs(countDifference); i++)
            {
                if (countDifference < 0)
                {
                    releaseParts.Add(0);
                }
                else
                {
                    otherReleaseParts.Add(0);
                }
            }

            for (var i = 0; i < ReleaseParts.Count; i++)
            {
                result = VersionHelper.CompareNumericValues(
                  ReleaseParts[i],
                  otherVersionInfo.ReleaseParts[i]
                );
                if (result != 0)
                {
                    break;
                }
            }

            return result;
        }

        private int CompareReleaseSuffixType(PythonVersionInfo otherVersionInfo)
        {
            return VersionHelper.CompareNumericValues(
              (int)ReleaseSuffixType,
              (int)otherVersionInfo.ReleaseSuffixType
            );
        }

        private int CompareIfDevelopmentReleaseSuffixType(
          PythonVersionInfo otherVersionInfo
        )
        {
            return ReleaseSuffixType == SuffixType.Development ?
              VersionHelper.CompareNumericValues(
                DevelopmentRelease.Increment,
                otherVersionInfo.DevelopmentRelease.Increment
              ) :
              0;
        }

        private int CompareIfPreReleaseSuffixType(
          PythonVersionInfo otherVersionInfo
        )
        {
            return ReleaseSuffixType == SuffixType.Pre ?
              ComparePreReleaseVersions(otherVersionInfo) :
              0;
        }

        private int CompareIfPostReleaseSuffixType(
          PythonVersionInfo otherVersionInfo
        )
        {
            return ReleaseSuffixType == SuffixType.Post ?
              ComparePostReleaseVersions(otherVersionInfo) :
              0;
        }

        private int ComparePreReleaseVersions(PythonVersionInfo otherVersionInfo)
        {
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
        )
        {
            if (PreReleaseSuffixType != (int)SuffixType.Post)
            {
                return 0;
            }

            int result = VersionHelper.CompareNumericValues(
              PostRelease.Increment,
              otherVersionInfo.PostRelease.Increment
            );
            return (result == 0) ?
              ComparePostReleaseVersions(otherVersionInfo) :
              result;
        }

        private int CompareDevelopmentReleaseIncrement(
          PythonVersionInfo otherVersionInfo
        )
        {
            return (PreReleaseSuffixType == (int)SuffixType.Development) ?
              VersionHelper.CompareNumericValues(
                DevelopmentRelease.Increment,
                otherVersionInfo.DevelopmentRelease.Increment
              ) :
              0;
        }

        private int ComparePreReleaseSuffixType(
          PythonVersionInfo otherVersionInfo
        )
        {
            return VersionHelper.CompareNumericValues(
              (int)PreReleaseSuffixType,
              (int)otherVersionInfo.PreReleaseSuffixType
            );
        }

        private int ComparePreReleaseIncrement(PythonVersionInfo otherVersionInfo)
        {
            return VersionHelper.CompareNumericValues(
              PreRelease.Increment,
              otherVersionInfo.PreRelease.Increment
            );
        }

        private int ComparePreReleaseLabel(PythonVersionInfo otherVersionInfo)
        {
            var result = string.Compare(
              PreRelease.Label,
              otherVersionInfo.PreRelease.Label,
              StringComparison.InvariantCulture
            );
            return result;
        }

        private int ComparePostReleaseVersions(PythonVersionInfo otherVersionInfo)
        {
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
        )
        {
            return (PostReleaseSuffixType == (int)SuffixType.Development) ?
              VersionHelper.CompareNumericValues(
                DevelopmentRelease.Increment,
                otherVersionInfo.DevelopmentRelease.Increment
              ) :
              0;
        }

        private int ComparePostReleaseSuffixType(
          PythonVersionInfo otherVersionInfo
        )
        {
            return VersionHelper.CompareNumericValues(
              (int)PostReleaseSuffixType,
              (int)otherVersionInfo.PostReleaseSuffixType
            );
        }

        private int ComparePostReleaseIncrement(
          PythonVersionInfo otherVersionInfo
        )
        {
            return VersionHelper.CompareNumericValues(
              PostRelease.Increment,
              otherVersionInfo.PostRelease.Increment
            );
        }

        private void ParseVersion()
        {
            ResetValuesToDefaults();

            if (!_versionExpression.IsMatch(Version))
            {
                throw new VersionParseException(Version);
            }

            var match = _versionExpression.Match(Version);

            Epoch = Conversions.SafeConvertToInt64(match.Groups[2].Value, 0);
            Release = Conversions.SafeExtractString(match.Groups[3].Value);
            ReleaseParts.AddRange(
              Conversions.SafeSplitIntoLongs(match.Groups[3].Value, '.')
            );

            PreRelease = BuildVersionPart(
              match.Groups[6].Value,
              match.Groups[7].Value,
              SuffixType.Pre
            );

            if (PreRelease != null)
            {
                ReleaseSuffix = PreRelease;
            }

            PostRelease = BuildVersionPart(
              match.Groups[9].Value,
              match.Groups[10].Value,
              SuffixType.Post
            );

            if (ReleaseSuffix != null && PostRelease != null)
            {
                ReleaseSuffix.Suffix = PostRelease;
            }
            else if (PostRelease != null)
            {
                ReleaseSuffix = PostRelease;
            }

            DevelopmentRelease = BuildVersionPart(
              "dev",
              match.Groups[13].Value,
              SuffixType.Development
            );

            if (ReleaseSuffix != null && DevelopmentRelease != null)
            {
                ReleaseSuffix.Suffix = DevelopmentRelease;
            }
            else if (DevelopmentRelease != null)
            {
                ReleaseSuffix = DevelopmentRelease;
            }
        }

        private PythonVersionPart BuildVersionPart(
          string label,
          string increment,
          SuffixType suffixType
        )
        {
            var convertedLabel = Conversions.SafeToLower(label);
            var convertedIncrement = Conversions.SafeConvertToInt64(increment, null);
            if (convertedIncrement.HasValue)
            {
                return new PythonVersionPart
                {
                    Label = convertedLabel,
                    Increment = convertedIncrement,
                    Type = suffixType
                };
            }

            return null;
        }

        private void ResetValuesToDefaults()
        {
            Epoch = 0;
            Release = null;
            ReleaseParts = new List<long>();
            PreRelease = null;
            PostRelease = null;
            DevelopmentRelease = null;
        }

        private void SetSuffixTypes()
        {
            if (!IsDevelopmentRelease && !IsPreRelease && !IsPostRelease)
            {
            }
            else if (IsPreRelease)
            {
                SetPreReleaseSuffixType();
            }
            else if (IsDevelopmentRelease && !IsPreRelease && !IsPostRelease)
            {
            }
            else if (IsPostRelease && !IsPreRelease)
            {
                SetPostReleaseSuffixType();
            }
        }

        private void SetPreReleaseSuffixType()
        {
            if (!IsDevelopmentRelease && !IsPostRelease)
            {
                PreReleaseSuffixType = (int)SuffixType.NoSuffix;
            }
            else if (IsDevelopmentRelease && !IsPostRelease)
            {
                PreReleaseSuffixType = (int)SuffixType.Development;
            }
            else if (IsPostRelease)
            {
                PreReleaseSuffixType = (int)SuffixType.Post;
                SetPostReleaseSuffixType();
            }
        }

        private void SetPostReleaseSuffixType()
        {
            PostReleaseSuffixType = IsDevelopmentRelease ?
              (int)SuffixType.Development :
              (int)SuffixType.NoSuffix;
        }

        public void RemoveLastReleaseIncrement()
        {
            if (Release.Contains("."))
            {
                Version = Release = Release.Remove(
                  Release.LastIndexOf(".", StringComparison.Ordinal)
                );
                ReleaseSuffix = null;
            }
        }

        public override string ToString()
        {
            return
              $"{nameof(Version)}: {Version}, " +
              $"{nameof(DatePublished)}: {DatePublished:d}";
        }
    }
}
