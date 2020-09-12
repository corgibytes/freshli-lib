using System;

namespace Freshli.Exceptions {
  public class VersionsBetweenNotFoundException : Exception {

    public VersionsBetweenNotFoundException(string dependency,
      string earlierVersion, string laterVersion, Exception e)
      : base($"Unable to find versions of " +
        $"{dependency} between {earlierVersion} and {laterVersion}.", e)
    {
    }

  }
}

