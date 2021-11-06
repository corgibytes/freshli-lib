using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IFileHistoryFinderRegistry
    {
        IList<IFileHistoryFinder> Finders { get; }

        void Register<TFinder>() where TFinder : IFileHistoryFinder, new();
    }
}
