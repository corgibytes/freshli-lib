using System;

namespace Freshli.Languages.Ruby {
  public class RubyGemsVersion {
    public string Authors { get; set; }
    public DateTime BuiltAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }
    public int DownloadsCount { get; set; }
    public string Number { get; set; }
    public string Summary { get; set; }
    public string Platform { get; set; }
    public string RubyVersion { get; set; }
    public bool Prerelease { get; set; }
    public string Licenses { get; set; }
    public string Requirements { get; set; }
    public string Sha { get; set; }
  }
}
