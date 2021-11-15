using System.Xml;

namespace Corgibytes.Freshli.Lib.Languages.CSharp
{
    public class NuGetManifest : AbstractManifest
    {
        protected override IManifestParser Parser => new NuGetManifestParser();
    }
}
