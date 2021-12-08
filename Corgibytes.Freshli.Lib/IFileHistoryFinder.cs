namespace Corgibytes.Freshli.Lib
{
    public interface IFileHistoryFinder
    {
        IFileHistorySource HistorySourceFor(string locator);
    }
}
