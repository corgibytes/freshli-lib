using System.Collections;
using System.Collections.Generic;

namespace Freshli.Languages.Perl {
  public class CpanfileManifest : AbstractManifest {
    public override void Parse(string contents) {
      throw new System.NotImplementedException();
    }

    public override bool UsesExactMatches => false;
  }
}
