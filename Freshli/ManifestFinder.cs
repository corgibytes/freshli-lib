using System.Collections.Generic;
using System.Collections.Immutable;

namespace Freshli
{
  public class ManifestFinder
  {
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

    public static void Register<TFinder>()
      where TFinder : IManifestFinder, new()
    {
      Finders.Add(new TFinder());
    }
  }
}
