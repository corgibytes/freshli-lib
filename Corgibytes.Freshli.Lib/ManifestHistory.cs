using System;
using System.Collections.Generic;
using System.IO;

namespace Corgibytes.Freshli.Lib;

public class ManifestHistory : IManifestHistory
{
    private string _filePath;
    private IFileHistory _fileHistory;
    private IManifestParser _parser;
    private IPackageRepository _repository;

    public ManifestHistory(string filePath, IFileHistory fileHistory, IManifestParser parser, IPackageRepository repository)
    {
        _filePath = filePath;
        _fileHistory = fileHistory;
        _parser = parser;
        _repository = repository;
    }

    public string FilePath => _filePath;

    public IEnumerable<DateTimeOffset> Dates => _fileHistory.Dates;

    public IManifest ManifestAsOf(DateTimeOffset asOf)
    {
        var sha = _fileHistory.ShaAsOf(asOf);
        var packages = _parser.Parse(_fileHistory.ContentStreamAsOf(asOf));
        return new Manifest(_repository, packages, VersionMatchMode.Exact, sha, _filePath);
    }
}
