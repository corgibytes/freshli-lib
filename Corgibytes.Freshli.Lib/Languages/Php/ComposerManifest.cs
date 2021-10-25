using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Microsoft.Extensions.Logging;

namespace Corgibytes.Freshli.Lib.Languages.Php
{
    public class ComposerManifest : AbstractManifest
    {
        public ComposerManifest(ILogger<IManifest> logger): base(logger)
        {
        }

        public override void Parse(string contents)
        {
            Clear();
            var options = new JsonDocumentOptions() { AllowTrailingCommas = true };
            using (var composerData = JsonDocument.Parse(contents, options))
            {
                var packages = composerData.RootElement.GetProperty("packages");
                AddPackagesFromJson(packages);

                var devPackages = composerData.RootElement.GetProperty("packages-dev");
                AddPackagesFromJson(devPackages);
            }
        }

        private void AddPackagesFromJson(JsonElement packages)
        {
            foreach (var package in packages.EnumerateArray())
            {
                var name = package.GetProperty("name").GetString();
                var version = package.GetProperty("version").GetString();
                Add(name, version);
            }
        }

        public override bool UsesExactMatches => true;
    }
}
