using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;

namespace Corgibytes.Freshli.Lib
{
    public class ManifestFinderRegistryLoader
    {
        private ILogger _logger;

        public ManifestFinderRegistryLoader(ILogger logger)
        {
            _logger = logger;
        }

        public void RegisterAll(IManifestFinderRegistry registry)
        {
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
                _logger.Log(
                    LogLevel.Info,
                    $"Registering AbstractManifestFinder: {type}"
                );
                registry.Register((AbstractManifestFinder)Activator.CreateInstance(type));
            }
        }

        private IEnumerable<Type> FindersLoadedIn(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes().
                    Where(
                        type => type.BaseType == typeof(AbstractManifestFinder) &&
                                type.GetConstructor(Type.EmptyTypes) != null
                    );
            }
            catch
            {
                _logger.Log(LogLevel.Info, $"Unable to load types from {assembly}");
                return new List<Type>();
            }
        }
    }
}
