using System;

namespace Freshli {
  public interface IVersionInfo : IComparable {

    public string Version  { get; }

    public DateTimeOffset DatePublished { get; }

    public bool IsPreRelease { get; }

  }
}
