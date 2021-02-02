using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using NLog;

namespace Freshli {
  public class ManifestFinder {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private readonly string _projectRootPath;

    private static readonly IList<AbstractManifestFinder> _finders =
      new List<AbstractManifestFinder>();

    public static IList<AbstractManifestFinder> Finders => _finders;

    public AbstractManifestFinder Finder { get; }

    public string[] ManifestFiles =>
      Finder.GetManifestFilenames(_projectRootPath);

    public bool Successful { get; }

    public LibYearCalculator Calculator => new LibYearCalculator(
      Finder.RepositoryFor(_projectRootPath),
      Finder.ManifestFor(_projectRootPath)
    );

    public ManifestFinder(
      string projectRootPath,
      IFileHistoryFinder fileFinder
    ) {
      _projectRootPath = projectRootPath;
      Successful = false;
      foreach (var finder in Finders.ToImmutableList()) {
        finder.FileFinder = fileFinder;
        if (finder.GetManifestFilenames(projectRootPath).Any()) {
          Finder = finder;
          Successful = true;
          break;
        }
      }
    }

    public static void Register(AbstractManifestFinder finder) {
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
        logger.Log(LogLevel.Info, $"Registering AbstractManifestFinder: {type}");
        Register((AbstractManifestFinder) Activator.CreateInstance(type));
      }
    }

    private static IEnumerable<Type> FindersLoadedIn(Assembly assembly) {
      try {
        return assembly.GetTypes().
          Where(
            type => type.BaseType == typeof(AbstractManifestFinder) &&
              type.GetConstructor(Type.EmptyTypes) != null
          );
      } catch {
        logger.Log(LogLevel.Info, $"Unable to load types from {assembly}");
        return new List<Type>();
      }
    }
  }
}
