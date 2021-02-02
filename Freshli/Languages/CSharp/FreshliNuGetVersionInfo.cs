using System;
using NuGet.Protocol.Core.Types;

namespace Freshli.Languages.CSharp {
  /**
  *  This serves as a wrapper for the 'NuGetVersion' class in the NuGet
  *  Client SDK.
  */
  public class FreshliNuGetVersionInfo : IVersionInfo {
    private IPackageSearchMetadata _packageSearchMetadata;

    public FreshliNuGetVersionInfo(IPackageSearchMetadata packageSearchMetadata)
    {
        _packageSearchMetadata = packageSearchMetadata;
    }

    public string Version => _packageSearchMetadata.Identity.Version.ToString();

    public DateTime DatePublished =>
        _packageSearchMetadata.Published.Value.DateTime;

    public bool IsPreRelease =>
        _packageSearchMetadata.Identity.Version.IsPrerelease;

    public int CompareTo(object obj) {
      //throw new NotImplementedException();
      return 0;
    }
  }
}
