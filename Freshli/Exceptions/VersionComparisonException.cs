using System;

namespace Freshli.Exceptions {
  public class VersionComparisonException : Exception {

    public VersionComparisonException(
      string version, string otherVersion, Exception e)
      : base($"Unable to compare versions " +
        $"{version} and {otherVersion}.", e)
    {
    }
  }
}
