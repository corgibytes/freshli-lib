namespace Corgibytes.Freshli.Lib
{
    public interface IFileHistoryFinder
    {
        // TODO: Create an async version of this method
        bool DoesPathContainHistorySource(string projectRootPath);
        // TODO: Create an async version of this method
        IFileHistory FileHistoryOf(string projectRootPath, string targetFile);
        // TODO: Create an async version of this method
        bool Exists(string projectRootPath, string filePath);
        // TODO: Create an async version of this method
        string ReadAllText(string projectRootPath, string filePath);
        // TODO: Create an async version of this method
        string[] GetManifestFilenames(string projectRootPath, string pattern);
    }
}
