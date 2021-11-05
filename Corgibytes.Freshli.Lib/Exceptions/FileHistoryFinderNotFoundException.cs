using System;

namespace Corgibytes.Freshli.Lib.Exceptions
{
    public class FileHistoryFinderNotFoundException : Exception
    {

        public FileHistoryFinderNotFoundException(string path)
          : base($"Unable to find an IFileHistoryFinder instance for {path}.")
        {
        }

    }
}
