using System.Collections;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    // TODO: Add logic to ensure that LocalFileHistoryFinder is always last in the list
    public class FileHistoryFinderRegistry : IFileHistoryFinderRegistry
    {
        private readonly IList<IFileHistoryFinder> _finders = new List<IFileHistoryFinder>();

        public IList<IFileHistoryFinder> Finders => _finders;

        public void Register<TFinder>() where TFinder : IFileHistoryFinder, new()
        {
            Finders.Add(new TFinder());
        }
    }
}
