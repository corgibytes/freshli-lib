using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IRunner
    {
        public IEnumerable<ScanResult> Run(string analysisPath, DateTimeOffset asOf);
        public IEnumerable<ScanResult> Run(string analysisPath);
    }
}
