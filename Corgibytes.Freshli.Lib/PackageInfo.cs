namespace Corgibytes.Freshli.Lib
{
    // TODO: Explore converting this `class` into a `record`
    public class PackageInfo
    {
        public string Name { get; }
        public string Version { get; }

        public PackageInfo(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }
}
