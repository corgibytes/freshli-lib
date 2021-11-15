using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Corgibytes.Freshli.Lib.Languages.Ruby
{
    public class BundlerManifest : AbstractManifest
    {
        protected override IManifestParser Parser => new BundlerManifestParser();
    }
}
