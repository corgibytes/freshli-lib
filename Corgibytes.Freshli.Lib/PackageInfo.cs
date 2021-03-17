namespace Corgibytes.Freshli.Lib {
  public class PackageInfo {
    public string Name { get; }
    public string Version { get; }

    public PackageInfo(string name, string version) {
      Name = name;
      Version = version;
    }
  }
}
