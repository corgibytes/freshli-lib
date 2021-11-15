using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace Corgibytes.Freshli.Lib.Languages.Ruby
{
    public class BundlerManifest : AbstractManifest
    {
        protected override IManifestParser Parser => new BundlerManifestParser();
    }
}
