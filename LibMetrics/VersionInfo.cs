using System;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace LibMetrics
{
  public class VersionInfo: IComparable
  {
    private string _version;

    public string Version
    {
      get => _version;
      set
      {
        ParseVersion(value);
        _version = value;
      }
    }

    public int Major { get; private set; }
    public int? Minor { get; private set; }
    public int? Patch { get; private set; }

    public string PreRelease
    {
      get => _preRelease;
      private set
      {
        ParsePreRelease(value);
        _preRelease = value;
      }
    }

    public string PreReleaseLabel { get; private set; }
    public int? PreReleaseIncrement { get; private set; }
    public string BuildMetadata { get; private set; }

    private Regex versionExpression = new Regex(@"^(0|[1-9]\d*)\.?(0|[1-9]\d*)?\.?(0|[1-9]\d*)?(?:-?((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$");
    private Regex preReleaseExpression = new Regex(@"([a-zA-Z-]+)\.?(\d*)");
    private string _preRelease;

    private void ParsePreRelease(string value)
    {
      if (value != null)
      {
        var match = preReleaseExpression.Match(value);
        PreReleaseLabel = match.Groups[1].Value;
        var incrementValue = match.Groups[2].Value;
        if (!string.IsNullOrWhiteSpace(incrementValue))
        {
          PreReleaseIncrement = Convert.ToInt32(incrementValue);
        }
      }
    }

    private void ParseVersion(string value)
    {
      var match = versionExpression.Match(value);
      Major = Convert.ToInt32(match.Groups[1].Value);

      var minorValue = match.Groups[2].Value;
      Minor = null;
      if (!string.IsNullOrWhiteSpace(minorValue))
      {
        Minor = Convert.ToInt32(minorValue);
      }

      var patchValue = match.Groups[3].Value;
      Patch = null;
      if (!string.IsNullOrWhiteSpace(patchValue))
      {
        Patch = Convert.ToInt32(patchValue);
      }

      var preReleaseValue = match.Groups[4].Value;
      PreRelease = null;
      if (!string.IsNullOrWhiteSpace(preReleaseValue))
      {
        PreRelease = preReleaseValue;
      }

      var buildMetadataValue = match.Groups[5].Value;
      BuildMetadata = null;
      if (!string.IsNullOrWhiteSpace(buildMetadataValue))
      {
        BuildMetadata = buildMetadataValue;
      }
    }

    public DateTime DatePublished { get; set; }

    public VersionInfo() {}

    public VersionInfo(string version, DateTime datePublished)
    {
      Version = version;
      DatePublished = datePublished;
    }

    public int CompareTo(object? other)
    {
      var otherVersionInfo = other as VersionInfo;
      if (otherVersionInfo == null)
      {
        throw new ArgumentException();
      }

      var result = Major.CompareTo(otherVersionInfo.Major);
      if (result != 0)
      {
        return result;
      }

      if (Minor.HasValue && otherVersionInfo.Minor.HasValue)
      {
        result = Minor.Value.CompareTo(otherVersionInfo.Minor.Value);
      }
      else if (Minor.HasValue)
      {
        result = 1;
      }
      else if (otherVersionInfo.Minor.HasValue)
      {
        result = -1;
      }
      if (result != 0)
      {
        return result;
      }

      if (Patch.HasValue && otherVersionInfo.Patch.HasValue)
      {
        result = Patch.Value.CompareTo(otherVersionInfo.Patch.Value);
      }
      else if (Patch.HasValue)
      {
        result = 1;
      }
      else if (otherVersionInfo.Patch.HasValue)
      {
        result = -1;
      }
      if (result != 0)
      {
        return result;
      }

      if (PreRelease != null && otherVersionInfo.PreRelease != null)
      {
        result = String.Compare(PreReleaseLabel, otherVersionInfo.PreReleaseLabel, StringComparison.Ordinal);

        if (result != 0)
        {
          return result;
        }

        if (PreReleaseIncrement.HasValue && otherVersionInfo.PreReleaseIncrement.HasValue)
        {
          result = PreReleaseIncrement.Value.CompareTo(otherVersionInfo.PreReleaseIncrement.Value);
        }
        else if (PreReleaseIncrement.HasValue)
        {
          result = 1;
        }
        else if (otherVersionInfo.PreReleaseIncrement.HasValue)
        {
          result = -1;
        }
      }
      else if (PreRelease != null)
      {
        result = -1;
      }
      else if (otherVersionInfo.PreRelease != null)
      {
        result = 1;
      }
      if (result != 0)
      {
        return result;
      }

      return DatePublished.CompareTo(otherVersionInfo.DatePublished);
    }
  }
}
