using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib.Languages.Python
{
    public class PipRequirementsTxtManifestFinder : IManifestFinder
    {
        public IEnumerable<IManifestHistory> GetManifests(IFileHistorySource fileHistorySource)
        {
            var files = fileHistorySource.GetManifestFilenames("requirements.txt");
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
            return new PipRequirementsTxtManifestParser();
        }

        protected virtual IPackageRepository CreateRepository()
        {
            // TODO: this should be injected
            return new PyPIRepository();
        }
    }
}
