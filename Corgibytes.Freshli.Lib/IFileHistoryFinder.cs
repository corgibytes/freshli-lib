namespace Corgibytes.Freshli.Lib
{
    public interface IFileHistoryFinder
    {
        // TODO: Create an async version of this method
        bool DoesProjectRootContainHistorySource();
        // TODO: Create an async version of this method
        IFileHistory FileHistoryOf(string targetFile);
        // TODO: Create an async version of this method
        bool Exists(string filePath);
        // TODO: Create an async version of this method
        string ReadAllText(string filePath);
        // TODO: Create an async version of this method
        string[] GetManifestFilenames(string pattern);
    }
}
