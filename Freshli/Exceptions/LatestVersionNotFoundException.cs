using System;

namespace Freshli.Exceptions {
  public class LatestVersionNotFoundException : Exception {

    public LatestVersionNotFoundException(DateTime date, string dependency)
      : base($"Unable to find latest version of dependency {dependency} as of {date:d}.")
    {
    }

  }
}
