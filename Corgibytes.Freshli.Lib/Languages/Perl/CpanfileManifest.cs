using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Corgibytes.Freshli.Lib.Languages.Perl
{
    public class CpanfileManifest : AbstractManifest
    {
        protected override IManifestParser Parser => new CpanfileManifestParser();
    }
}
