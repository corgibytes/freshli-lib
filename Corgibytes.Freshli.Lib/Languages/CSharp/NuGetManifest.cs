using System.Xml;

namespace Corgibytes.Freshli.Lib.Languages.CSharp {
  public class NuGetManifest : AbstractManifest {
    public override void Parse(string contents) {
      var xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(contents);

      var packages = xmlDoc.GetElementsByTagName("PackageReference");
      foreach (XmlNode package in packages)
      {
        Add(
          package.Attributes[0].Value,
          package.Attributes[1].Value
        );
      }
    }

    public override bool UsesExactMatches => true;
  }
}
