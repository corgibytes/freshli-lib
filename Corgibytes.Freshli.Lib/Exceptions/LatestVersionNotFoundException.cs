using System;

namespace Corgibytes.Freshli.Lib.Exceptions
{
    public class LatestVersionNotFoundException : Exception
    {

        public LatestVersionNotFoundException(
          string dependency,
          DateTimeOffset date,
          Exception exception
        ) : base(
          $"Unable to find latest version of {dependency} as of {date:O}.",
          exception
        )
        {
        }
    }
}
