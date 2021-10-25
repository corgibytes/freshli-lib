using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface ICalculatorService
    {
        IEnumerable<MetricsResult> Compute(IFileHistory fileHistory, IAnalysisDates analysisDates);
    }
}
