using LibGit2Sharp;

namespace LibMetrics
{
  public class GitFileHistoryFinder: IFileHistoryFinder
  {
    public bool DoesPathContainHistorySource(string projectRootPath)
    {
      return Repository.IsValid(projectRootPath);
    }

    public IFileHistory FileHistoryOf(string projectRootPath, string targetFile)
    {
      return new GitFileHistory(projectRootPath, targetFile);
    }
  }
}
