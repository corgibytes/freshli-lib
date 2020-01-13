using System;

namespace LibMetrics
{
  public class VersionInfo
  {
    public string Version { get; }
    public DateTime DatePublished { get; }

    public VersionInfo(string version, DateTime datePublished)
    {
      Version = version;
      DatePublished = datePublished;
    }
  }
}
