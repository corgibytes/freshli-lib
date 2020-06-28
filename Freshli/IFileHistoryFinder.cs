namespace Freshli {
  public interface IFileHistoryFinder {
    bool DoesPathContainHistorySource(string projectRootPath);
    IFileHistory FileHistoryOf(string projectRootPath, string targetFile);
    bool Exists(string projectRootPath, string filePath);
    string ReadAllText(string projectRootPath, string filePath);
  }
}
