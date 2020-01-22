using System.Collections.Generic;

namespace LibMetrics
{
  public class FileHistoryFinder: IFileHistoryFinder
  {
    private readonly string _projectRootPath;

    private static readonly IList<IFileHistoryFinder> _finders =
      new List<IFileHistoryFinder>();

    public static IList<IFileHistoryFinder> Finders => _finders;
    public IFileHistoryFinder Finder { get; }

    public FileHistoryFinder(string projectRootPath)
    {
      _projectRootPath = projectRootPath;
      Finder = this;
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

    public bool DoesPathContainHistorySource(string projectRootPath)
    {
      return true;
    }

    public IFileHistory FileHistoryOf(string projectRootPath, string targetFile)
    {
      return new LocalFileHistory(projectRootPath, targetFile);
    }
  }
}
