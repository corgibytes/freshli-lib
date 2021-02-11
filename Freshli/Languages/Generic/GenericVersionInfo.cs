using System;

namespace Freshli.Languages.Generic {
  public class GenericVersionInfo : IVersionInfo {
    public int CompareTo(object? obj) {
      throw new NotImplementedException();
    }

    public string Version { get; }
    public DateTime DatePublished { get; }
    public bool IsPreRelease { get; }
  }
}
