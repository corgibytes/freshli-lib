namespace Corgibytes.Freshli.Lib
{
    public interface IFileHistoryService
    {
        IFileHistoryFinder SelectFinderFor(string projectRootPath);
    }
}
