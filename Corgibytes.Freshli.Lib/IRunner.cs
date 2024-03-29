﻿using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IRunner
    {
        public IList<ScanResult> Run(string analysisPath, DateTimeOffset asOf);
        public IList<ScanResult> Run(string analysisPath);
    }
}
