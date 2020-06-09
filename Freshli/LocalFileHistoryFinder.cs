using System.IO;

namespace Freshli
{
  public class LocalFileHistoryFinder: IFileHistoryFinder
  {
    public bool DoesPathContainHistorySource(string projectRootPath)
    {
      return true;
    }

    public IFileHistory FileHistoryOf(string projectRootPath, string targetFile)
    {
      return new LocalFileHistory(projectRootPath, targetFile);
    }

    public bool Exists(string projectRootPath, string filePath)
    {
      return File.Exists(Path.Combine(projectRootPath, filePath));
    }

    public string ReadAllText(string projectRootPath, string filePath)
    {
      return File.ReadAllText(Path.Combine(projectRootPath, filePath));
    }
  }
}
