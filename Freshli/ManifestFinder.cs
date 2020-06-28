using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using NLog;

namespace Freshli
{
  public class ManifestFinder
  {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private readonly string _projectRootPath;

    private static readonly IList<IManifestFinder> _finders =
      new List<IManifestFinder>();
    public static IList<IManifestFinder> Finders => _finders;

    public IManifestFinder Finder { get; }

    public string LockFileName => Finder.LockFileName;
    public bool Successful { get; }

    public LibYearCalculator Calculator => new LibYearCalculator(
      Finder.RepositoryFor(_projectRootPath),
      Finder.ManifestFor(_projectRootPath));

    public ManifestFinder(string projectRootPath, IFileHistoryFinder fileFinder)
    {
      _projectRootPath = projectRootPath;
      Successful = false;
      foreach (var finder in Finders.ToImmutableList())
      {
        finder.FileFinder = fileFinder;
        if (finder.DoesPathContainManifest(projectRootPath))
        {
          Finder = finder;
          Successful = true;
          break;
        }
      }
    }

    public static void Register(IManifestFinder finder) {
      Finders.Add(finder);
    }

    public static void RegisterAll() {
      var manifestFinderTypes = new HashSet<Type>();
      foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
        foreach (var type in FindersLoadedIn(assembly)) {
          manifestFinderTypes.Add(type);
        }
      }
      
      foreach (var type in manifestFinderTypes) {
        logger.Log(LogLevel.Info, $"Registering IManifestFinder: {type}");
        Register((IManifestFinder) Activator.CreateInstance(type));
      }      
    }

    private static IEnumerable<Type> FindersLoadedIn(Assembly assembly) {
      return assembly.GetTypes().
        Where(
          type => type.GetInterfaces().Contains(typeof(IManifestFinder)) &&
            type.GetConstructor(Type.EmptyTypes) != null
        ); 
    }
  }
}
