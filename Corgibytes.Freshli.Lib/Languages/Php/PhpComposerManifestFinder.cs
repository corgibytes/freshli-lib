using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib.Languages.Php
{
    public class PhpComposerManifestFinder : IManifestFinder
    {
        public IEnumerable<IManifestHistory> GetManifests(IFileHistorySource fileHistorySource)
        {
            var files = fileHistorySource.GetManifestFilenames("composer.lock");
            foreach (var lockFile in files)
            {
                var lockFileHistory = fileHistorySource.FileHistoryOf(lockFile);

                var fileExtensionStart = lockFile.LastIndexOf(".lock");
                var jsonFile = lockFile.Remove(fileExtensionStart) + ".json";
                var jsonFileHistory = fileHistorySource.FileHistoryOf(jsonFile);

                var parser = CreateManifestParser();
                var repository = new MulticastComposerRepository(jsonFileHistory);
                yield return new PhpManifestHistory(lockFile, jsonFileHistory, lockFileHistory, parser, repository);
            }
        }

        protected virtual IManifestParser CreateManifestParser()
        {
            // TODO: this should be injected
            return new ComposerManifestParser();
        }
    }
}
