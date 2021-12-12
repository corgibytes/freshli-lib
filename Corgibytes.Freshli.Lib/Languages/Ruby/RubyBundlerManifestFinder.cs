using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib.Languages.Ruby
{
    public class RubyBundlerManifestFinder : IManifestFinder
    {
        public IEnumerable<IManifestHistory> GetManifests(IFileHistorySource fileHistorySource)
        {
            var files = fileHistorySource.GetManifestFilenames("Gemfile.lock");
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
            return new BundlerManifestParser();
        }

        protected virtual IPackageRepository CreateRepository()
        {
            // TODO: this should be injected
            return new RubyGemsRepository();
        }
    }
}
