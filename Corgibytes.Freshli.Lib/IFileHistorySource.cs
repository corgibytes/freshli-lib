namespace Corgibytes.Freshli.Lib
{
    public interface IFileHistorySource
    {
        bool ContainsFileHistory { get; }
        IFileHistory FileHistoryOf(string targetFile);
        bool Exists(string filePath);
        string ReadAllText(string filePath);
        string[] GetManifestFilenames(string pattern);
    }
}
