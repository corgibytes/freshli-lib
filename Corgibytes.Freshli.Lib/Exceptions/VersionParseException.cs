using System;

namespace Corgibytes.Freshli.Lib.Exceptions
{
    public class VersionParseException : Exception
    {

        public VersionParseException(string version)
          : base($"Unable to parse version string: '{version}'.")
        {
        }
    }
}
