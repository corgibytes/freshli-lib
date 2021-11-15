using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using NLog;

namespace Corgibytes.Freshli.Lib.Languages.Php
{
    public class ComposerManifest : AbstractManifest
    {
        protected override IManifestParser Parser => new ComposerManifestParser();
    }
}
