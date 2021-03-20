using System;

namespace Corgibytes.Freshli.Lib.Exceptions {
  public class LatestVersionThatMatchesNotFoundException : Exception {

    public LatestVersionThatMatchesNotFoundException(
      string dependency, DateTimeOffset date, string matchPattern, Exception e)
      : base($"Unable to find latest version of {dependency}" +
        $" that matches '{matchPattern}' as of {date:O}.", e)
    {
    }

  }
}
