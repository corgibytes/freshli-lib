namespace Corgibytes.Freshli.Lib
{
    public interface IFileHistoryFinder
    {
        // Move project root path to a property value and take it out of the method calls
        bool DoesPathContainHistorySource(string projectRootPath);
        IFileHistory FileHistoryOf(string projectRootPath, string targetFile);
        bool Exists(string projectRootPath, string filePath);
        string ReadAllText(string projectRootPath, string filePath);
        string[] GetManifestFilenames(string projectRootPath, string pattern);
    }
}
