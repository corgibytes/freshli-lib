using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Logging;

namespace Corgibytes.Freshli.Lib
{
    public class ManifestFinderRegistry : IManifestFinderRegistry
    {
        private readonly ILogger<ManifestFinderRegistry> _logger;

        private readonly IList<AbstractManifestFinder> _finders = new List<AbstractManifestFinder>();

        public IList<AbstractManifestFinder> Finders => _finders;

        public ManifestFinderRegistry(ILogger<ManifestFinderRegistry> logger)
        {
            _logger = logger;
        }

        public void Register(AbstractManifestFinder finder)
        {
            _logger.LogTrace($"Registering manifest finder: {finder.GetType().AssemblyQualifiedName}");
            Finders.Add(finder);
        }
    }

}
