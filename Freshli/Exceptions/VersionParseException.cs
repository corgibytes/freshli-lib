using System;

namespace Freshli.Exceptions {
  public class VersionParseException : Exception {

    public VersionParseException(string version)
      : base($"Unable to parse version string: '{version}'.")
    {
    }
  }
}
