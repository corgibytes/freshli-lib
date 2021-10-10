using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IRunner
    {
        // TODO: Create an async version of this method
        public IList<ScanResult> Run(string analysisPath, DateTime asOf);
        // TODO: Create an async version of this method
        public IList<ScanResult> Run(string analysisPath);
    }
}
