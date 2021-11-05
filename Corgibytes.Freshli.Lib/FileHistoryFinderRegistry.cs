using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public class FileHistoryFinderRegistry
    {
        private static readonly IList<IFileHistoryFinder> _finders = new List<IFileHistoryFinder>();

        public static IList<IFileHistoryFinder> Finders => _finders;

        public static void Register<TFinder>() where TFinder : IFileHistoryFinder, new()
        {
            Finders.Add(new TFinder());
        }
    }
}
