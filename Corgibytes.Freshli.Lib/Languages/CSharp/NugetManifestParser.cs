using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Corgibytes.Freshli.Lib.Languages.CSharp
{
    public class NuGetManifestParser : IManifestParser
    {
        public bool UsesExactMatches => true;

        public IEnumerable<PackageInfo> Parse(Stream contentsStream)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(contentsStream);

            var packages = xmlDoc.GetElementsByTagName("PackageReference");
            foreach (XmlNode package in packages)
            {
                yield return new PackageInfo(
                    package.Attributes[0].Value,
                    package.Attributes[1].Value
                );
            }
        }

    }
}
