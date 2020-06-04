using LibGit2Sharp;

namespace LibMetrics.Languages.Python
{
  public class VersionMatcher
  {
    public enum OperationKind
    {
      Matching,
      PrefixMatching
    }

    public OperationKind Operation { get; private set; }
    public VersionInfo BaseVersion { get; private set; }

    public VersionMatcher(string value)
    {
      if (value.StartsWith("=="))
      {
        value = value.Remove(0, 2);
        Operation = OperationKind.Matching;

        if (value.EndsWith(".*"))
        {
          Operation = OperationKind.PrefixMatching;
          value = value.Replace(".*", "");
        }

        BaseVersion = new VersionInfo() {Version = value};
      }
    }

    public bool DoesMatch(VersionInfo version)
    {
      if (Operation == OperationKind.Matching)
      {
        return version.CompareTo(BaseVersion) == 0;
      }
      else if (Operation == OperationKind.PrefixMatching)
      {
        return version.Version.StartsWith(BaseVersion.Version) &&
               version.CompareTo(BaseVersion) >= 0;
      }


      return false;
    }
  }
}
