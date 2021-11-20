using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;

namespace Corgibytes.Freshli.Lib
{
    public class ManifestFinderRegistry
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IList<AbstractManifestFinder> _finders =
            new List<AbstractManifestFinder>();

        public IList<AbstractManifestFinder> Finders => _finders;

        public void Register(AbstractManifestFinder finder)
        {
            Finders.Add(finder);
        }

        public void RegisterAll()
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
                logger.Log(
                    LogLevel.Info,
                    $"Registering AbstractManifestFinder: {type}"
                );
                Register((AbstractManifestFinder)Activator.CreateInstance(type));
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
                logger.Log(LogLevel.Info, $"Unable to load types from {assembly}");
                return new List<Type>();
            }
        }


    }
}
