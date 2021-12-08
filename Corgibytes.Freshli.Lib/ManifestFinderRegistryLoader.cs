using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Logging;

namespace Corgibytes.Freshli.Lib
{
    // TODO: This class can go away once the tests adopt a IoC container
    public class ManifestFinderRegistryLoader
    {
        private ILogger<ManifestFinderRegistryLoader> _logger;

        public ManifestFinderRegistryLoader(ILogger<ManifestFinderRegistryLoader> logger)
        {
            _logger = logger;
        }

        public void RegisterAll(IManifestFinderRegistry registry)
        {
            _logger.LogTrace("RegisterAll");
            var manifestFinderTypes = new HashSet<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in FindersLoadedIn(assembly))
                {
                    manifestFinderTypes.Add(type);
                }
            }

            foreach (var type in manifestFinderTypes)
            {
                _logger.LogDebug($"Registering IManifestFinder: {type}");
                registry.Register((IManifestFinder)Activator.CreateInstance(type));
            }
        }

        private IEnumerable<Type> FindersLoadedIn(Assembly assembly)
        {
            _logger.LogDebug($"Looking for IManifestFinder implementations in #{assembly}");
            var foundTypes = assembly.GetTypes().
                Where(
                    type => type.GetInterfaces().Contains(typeof(IManifestFinder)) &&
                            type.GetConstructor(Type.EmptyTypes) != null
                );

            foreach (var type in foundTypes)
            {
                yield return type;
            }
        }
    }
}
