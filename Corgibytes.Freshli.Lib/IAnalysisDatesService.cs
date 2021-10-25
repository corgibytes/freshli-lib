using System;

namespace Corgibytes.Freshli.Lib
{
    public interface IAnalysisDatesService
    {
        IAnalysisDates DetermineDatesFor(IFileHistory fileHistory, DateTime asOf);
    }
}


// TODO: Rebase this branch on top of the date-time-offset changes to make it dependent on them