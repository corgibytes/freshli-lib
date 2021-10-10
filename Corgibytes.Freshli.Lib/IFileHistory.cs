using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IFileHistory
    {
        // TODO: Create an async version of this. Will need to make it a regular method.
        IList<DateTime> Dates { get; }
        // TODO: Create an async version of this method
        string ContentsAsOf(DateTime date);
        // TODO: Create an async version of this method
        string ShaAsOf(DateTime date);
    }
}
