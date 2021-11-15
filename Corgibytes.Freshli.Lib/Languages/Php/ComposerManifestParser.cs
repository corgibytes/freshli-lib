using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Corgibytes.Freshli.Lib.Languages.Php
{
    public class ComposerManifestParser : IManifestParser
    {
        public bool UsesExactMatches => true;

        public IEnumerable<PackageInfo> Parse(Stream contentsStream)
        {
            var options = new JsonDocumentOptions() { AllowTrailingCommas = true };
            using (var composerData = JsonDocument.Parse(contentsStream, options))
            {
                var packages = composerData.RootElement.GetProperty("packages");
                foreach (var package in AddPackagesFromJson(packages))
                {
                    yield return package;
                }

                var devPackages = composerData.RootElement.GetProperty("packages-dev");
                foreach (var package in AddPackagesFromJson(devPackages))
                {
                    yield return package;
                }
            }
        }

        private IEnumerable<PackageInfo> AddPackagesFromJson(JsonElement packages)
        {
            foreach (var package in packages.EnumerateArray())
            {
                var name = package.GetProperty("name").GetString();
                var version = package.GetProperty("version").GetString();
                yield return new PackageInfo(name, version);
            }
        }

    }
}
