using System;
using System.Text.RegularExpressions;
using Corgibytes.Freshli.Lib.Exceptions;

namespace Corgibytes.Freshli.Lib
{
    /*
      SemVerVersionInfo assumes dependency versions follows the standards set
      forth by https://semver.org/.
  */
    public class SemVerVersionInfo : IVersionInfo
    {
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

        public string Version { get; private set; }
        public long Major { get; private set; }
        public long? Minor { get; private set; }
        public long? Patch { get; private set; }
        public DateTime DatePublished { get; set; }

        public string PreRelease
        {
            get => _preRelease;
            set
            {
                ParsePreRelease(value);
                _preRelease = value;
            }
        }

        public bool IsPreRelease { get; set; }
        public string PreReleaseLabel { get; private set; }
        public long? PreReleaseIncrement { get; private set; }
        public string BuildMetadata { get; private set; }

        public SemVerVersionInfo(string version, DateTime? datePublished = null)
        {
            Version = version;
            if (datePublished.HasValue) { DatePublished = datePublished.Value; }

            ParseVersion();
        }

        public int CompareTo(object other)
        {
            var otherVersionInfo = other as SemVerVersionInfo;
            if (otherVersionInfo == null)
            {
                throw new ArgumentException();
            }

            int result = 0;

            result = CompareMajor(otherVersionInfo.Major);
            if (result != 0) { return result; }

            result = CompareMinor(otherVersionInfo.Minor);
            if (result != 0) { return result; }

            result = ComparePatch(otherVersionInfo.Patch);
            if (result != 0) { return result; }

            // default to zero if everything matches.
            return ComparePrelease(otherVersionInfo);
        }

        // We are treating zero as either equal or doesn't exist, meaning
        // we should go to the next item.
        private int CompareMajor(long otherMajor)
        {
            return Major.CompareTo(otherMajor);
        }

        private int CompareMinor(long? otherMinor)
        {
            if (Minor.HasValue && otherMinor.HasValue)
            {
                return Minor.Value.CompareTo(otherMinor.Value);
            }

            if (Minor.HasValue)
            {
                return Minor.Value == 0 ? 0 : 1;
            }

            if (otherMinor.HasValue)
            {
                return otherMinor.Value == 0 ? 0 : -1;
            }

            return 0;
        }

        private int ComparePatch(long? otherPatch)
        {
            int result = 0;
            if (Patch.HasValue && otherPatch.HasValue)
            {
                result = Patch.Value.CompareTo(otherPatch.Value);
            }
            else if (Patch.HasValue)
            {
                result = 1;
                if (Patch.Value == 0)
                {
                    result = 0;
                }
            }
            else if (otherPatch.HasValue)
            {
                result = -1;
                if (otherPatch.Value == 0)
                {
                    result = 0;
                }
            }

            return result;
        }

        private int ComparePrelease(SemVerVersionInfo otherVersionInfo)
        {
            if (PreRelease == null && otherVersionInfo.PreRelease == null)
            {
                return 0;
            }
            if (PreRelease == null || otherVersionInfo.PreRelease == null)
            {
                return PreRelease != null ? -1 : 1;
            }

            int result = 0;
            result = String.Compare(
              PreReleaseLabel,
              otherVersionInfo.PreReleaseLabel,
              StringComparison.Ordinal
            );
            if (result != 0) { return result; }

            return ComparePrereleaseIncrement(otherVersionInfo);
        }

        private int ComparePrereleaseIncrement(SemVerVersionInfo otherVersionInfo)
        {
            if (PreReleaseIncrement.HasValue &&
                    otherVersionInfo.PreReleaseIncrement.HasValue)
            {
                return PreReleaseIncrement.Value.CompareTo(
                  otherVersionInfo.PreReleaseIncrement.Value
                );
            }
            else if (PreReleaseIncrement.HasValue)
            {
                return 1;
            }
            else if (otherVersionInfo.PreReleaseIncrement.HasValue)
            {
                return -1;
            }

            return 0;
        }

        private void ParseVersion()
        {

            if (!_versionExpression.IsMatch(Version))
            {
                throw new VersionParseException(Version);
            }

            var match = _versionExpression.Match(Version);
            Major = Convert.ToInt64(match.Groups[1].Value);
            Minor = ConvertToNullableNumber(match.Groups[2].Value);
            Patch = ConvertToNullableNumber(match.Groups[3].Value);
            PreRelease = ConvertToNullableString(match.Groups[4].Value);
            BuildMetadata = ConvertToNullableString(match.Groups[5].Value);
        }

        public override string ToString()
        {
            return
              $"{nameof(Major)}: {Major}, " +
              $"{nameof(Minor)}: {Minor}, " +
              $"{nameof(Patch)}: {Patch}, " +
              $"{nameof(PreRelease)}: {PreRelease}, " +
              $"{nameof(BuildMetadata)}: {BuildMetadata}, " +
              $"{nameof(DatePublished)}: {DatePublished:d}";
        }

        private void ParsePreRelease(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                IsPreRelease = true;
                var match = _preReleaseExpression.Match(value);
                PreReleaseLabel = match.Groups[1].Value;
                var incrementValue = match.Groups[2].Value;
                if (!string.IsNullOrWhiteSpace(incrementValue))
                {
                    PreReleaseIncrement = Convert.ToInt64(incrementValue);
                }
            }
            else
            {
                IsPreRelease = false;
                PreReleaseLabel = null;
                PreReleaseIncrement = null;
            }
        }

        private long? ConvertToNullableNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { return null; }

            return Convert.ToInt64(value);
        }

        private string ConvertToNullableString(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { return null; }

            return value;
        }
    }
}
