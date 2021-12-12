using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib;

public class Manifest : IManifest
{
    private IPackageRepository _repository;
    private IEnumerable<PackageInfo> _contents;
    private VersionMatchMode _matchMode;
    private string _revision;
    private string _filePath;

    public Manifest(IPackageRepository repository, IEnumerable<PackageInfo> contents, VersionMatchMode matchMode, string revision, string filePath)
    {
        _repository = repository;
        _contents = contents;
        _matchMode = matchMode;
        _revision = revision;
        _filePath = filePath;
    }

    public IPackageRepository Repository => _repository;

    public IEnumerable<PackageInfo> Contents => _contents;

    public bool UsesExactMatches => _matchMode == VersionMatchMode.Exact;

    public string Revision => _revision;

    public string FilePath => _filePath;
}

public enum VersionMatchMode
{
    Exact,
    Expression
}
