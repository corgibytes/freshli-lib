using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib.Languages.Perl
{
    public class CpanfileManifestFinder : IManifestFinder
    {
        public IEnumerable<IManifestHistory> GetManifests(IFileHistorySource fileHistorySource) => throw new System.NotImplementedException();
    }
}
