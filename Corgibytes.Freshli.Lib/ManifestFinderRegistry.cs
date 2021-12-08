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

        private readonly IList<IManifestFinder> _finders = new List<IManifestFinder>();

        public IList<IManifestFinder> Finders => _finders;

        public ManifestFinderRegistry(ILogger<ManifestFinderRegistry> logger)
        {
            _logger = logger;
        }

        public void Register(IManifestFinder finder)
        {
            _logger.LogTrace($"Registering manifest finder: {finder.GetType().AssemblyQualifiedName}");
            Finders.Add(finder);
        }
    }

}
