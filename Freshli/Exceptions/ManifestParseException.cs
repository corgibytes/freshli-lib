using System;

namespace Freshli.Exceptions {
  public class ManifestParseException : Exception {

    public ManifestParseException(string manifest)
      : base($"Unable to parse manifest string: '{manifest}'.")
    {
    }
  }
}
