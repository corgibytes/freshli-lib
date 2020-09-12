using System;

namespace Freshli {
  public interface IVersionInfo : IComparable {

    public string Version  { get; set; }

    public DateTime DatePublished { get; set; }

    public string ToSimpleVersion();

  }
}
