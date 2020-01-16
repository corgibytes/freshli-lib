namespace LibMetrics
{
  public interface IFileHistoryFinder
  {
    bool DoesPathContainHistorySource(string projectRootPath);
    IFileHistory FileHistoryOf(string projectRootPath, string targetFile);
  }
}
