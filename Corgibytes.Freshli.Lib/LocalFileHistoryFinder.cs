using System.IO;
using System.Linq;

namespace Corgibytes.Freshli.Lib
{
    public class LocalFileHistoryFinder : IFileHistoryFinder
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

        public string[] GetManifestFilenames(string projectRootPath, string pattern)
        {
            return Directory.
                GetFiles(projectRootPath, pattern, SearchOption.AllDirectories).
                Select(f => Path.GetFileName(f)).ToArray();
        }
    }
}
