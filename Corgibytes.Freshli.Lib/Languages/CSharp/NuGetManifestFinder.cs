using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Corgibytes.Freshli.Lib.Languages.CSharp
{
    public class NuGetManifestFinder : IManifestFinder
    {
        public IEnumerable<IManifestHistory> GetManifests(IFileHistorySource fileHistorySource) => throw new System.NotImplementedException();
    }
}
