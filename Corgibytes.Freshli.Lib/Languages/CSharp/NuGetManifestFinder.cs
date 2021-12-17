using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Corgibytes.Freshli.Lib.Languages.CSharp
{
    public class NuGetManifestFinder : IManifestFinder
    {
        public IEnumerable<IManifestHistory> GetManifests(IFileHistorySource fileHistorySource)
        {
            var files = fileHistorySource.GetManifestFilenames("*.csproj");
            foreach (var file in files)
            {
                var fileHistory = fileHistorySource.FileHistoryOf(file);
                var parser = CreateManifestParser();
                var repository = CreateRepository();
                yield return new ManifestHistory(file, fileHistory, parser, repository);
            }
        }

        protected virtual IManifestParser CreateManifestParser()
        {
            // TODO: this should be injected
            return new NuGetManifestParser();
        }

        protected virtual IPackageRepository CreateRepository()
        {
            // TODO: this should be injected
            return new NuGetRepository();
        }
    }
}
