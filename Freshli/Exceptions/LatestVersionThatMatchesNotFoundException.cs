using System;

namespace Freshli.Exceptions {
  public class LatestVersionThatMatchesNotFoundException : Exception {

    public LatestVersionThatMatchesNotFoundException(
      string dependency, DateTime date, string matchPattern, Exception e)
      : base($"Unable to find latest version of dependency " +
        $"{dependency} as of {date:d} that matches {matchPattern}.", e)
    {
    }

  }
}
