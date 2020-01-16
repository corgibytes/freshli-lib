using System.Collections.Generic;

namespace LibMetrics
{
  public class FileHistoryFinder
  {
    private readonly string _projectRootPath;

    private static readonly IList<IFileHistoryFinder> _finders =
      new List<IFileHistoryFinder>();

    private static IFileHistoryFinder _defaultFinder;
    public static IList<IFileHistoryFinder> Finders => _finders;
    public IFileHistoryFinder Finder { get; }

    public static IFileHistoryFinder DefaultFinder => _defaultFinder;

    public FileHistoryFinder(string projectRootPath)
    {
      _projectRootPath = projectRootPath;
      Finder = DefaultFinder;
      foreach(var finder in Finders)
      {
        if (finder.DoesPathContainHistorySource(projectRootPath))
        {
          Finder = finder;
          break;
        }
      }
    }

    public IFileHistory FileHistoryOf(string targetFile)
    {
      return Finder.FileHistoryOf(_projectRootPath, targetFile);
    }

    public static void Register<TFinder>()
      where TFinder : IFileHistoryFinder, new()
    {
      Finders.Add(new TFinder());
    }

    public static void RegisterDefault<TFinder>()
      where TFinder : IFileHistoryFinder, new()
    {
      _defaultFinder = new TFinder();
    }
  }
}
