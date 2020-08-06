using System;

namespace Freshli.Exceptions {
  public class LatestVersionNotFoundException : Exception {

    public LatestVersionNotFoundException(string dependency, DateTime date,
      Exception e)
      : base($"Unable to find latest version of " +
        $"{dependency} as of {date:d}.", e)
    {
    }

  }
}
