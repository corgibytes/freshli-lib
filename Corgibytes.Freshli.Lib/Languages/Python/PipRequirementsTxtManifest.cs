using System.IO;
using System.Text.RegularExpressions;

namespace Corgibytes.Freshli.Lib.Languages.Python
{
    public class PipRequirementsTxtManifest : AbstractManifest
    {
        protected override IManifestParser Parser => new PipRequirementsTxtManifestParser();
    }
}
