using System;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Freshli.Languages.CSharp {
  /**
  *  This serves as a wrapper for the 'NuGetVersion' class in the NuGet
  *  Client SDK.
  */
  public class FreshliNuGetVersionInfo : IVersionInfo {
    private NuGetVersion _nuGetVersion;
    private int Major => _nuGetVersion.Major;
    private int Minor => _nuGetVersion.Minor;
    private int Patch => _nuGetVersion.Patch;

    public DateTimeOffset DatePublished { get; init; }

    public FreshliNuGetVersionInfo(
      NuGetVersion nuGetVersion,
      DateTimeOffset datePublished
    ) {
        _nuGetVersion = nuGetVersion;
        DatePublished = datePublished;
    }

    public string Version => _nuGetVersion.ToString();

    public bool IsPreRelease => _nuGetVersion.IsPrerelease;

    public int CompareTo(object obj) {
      var other = obj as FreshliNuGetVersionInfo;
      if (other == null) {
        throw new ArgumentException("FreshliNuGetVersionInfo not provided " +
          "for CompareTo()");
      }

      var result = Major.CompareTo(other.Major);
      if (result != 0) { return result; }

      result = Minor.CompareTo(other.Minor);
      if (result != 0) { return result; }

      result = Patch.CompareTo(other.Patch);
      if (result != 0) { return result; }

      return 0;
    }
  }
}
