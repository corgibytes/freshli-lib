using System.Collections.Generic;
using System.IO;

namespace LibMetrics
{
  public class FileHistoryFinder
  {
    private readonly string _projectRootPath;

    private static readonly IList<IFileHistoryFinder> _finders =
      new List<IFileHistoryFinder>();

    public static IList<IFileHistoryFinder> Finders => _finders;
    public IFileHistoryFinder Finder { get; }

    public FileHistoryFinder(string projectRootPath)
    {
      _projectRootPath = projectRootPath;
      Finder = new LocalFileHistoryFinder();
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
  }
}
