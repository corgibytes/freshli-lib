using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;

namespace Corgibytes.Freshli.Lib
{
    public class ManifestFinderRegistry : IManifestFinderRegistry
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IList<AbstractManifestFinder> _finders =
            new List<AbstractManifestFinder>();

        public IList<AbstractManifestFinder> Finders => _finders;

        public void Register(AbstractManifestFinder finder)
        {
            Finders.Add(finder);
        }
    }

}
