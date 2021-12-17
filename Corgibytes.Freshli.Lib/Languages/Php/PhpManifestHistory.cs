using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib.Languages.Php;

public class PhpManifestHistory : IManifestHistory
{
    private string _filePath;
    private IFileHistory _jsonFileHistory;
    private IFileHistory _lockFileHistory;
    private IManifestParser _parser;
    private IPackageRepository _repository;
    private IEnumerable<DateTimeOffset> _combinedDates;

    public PhpManifestHistory(string filePath, IFileHistory jsonFileHistory, IFileHistory lockFileHistory, IManifestParser parser, IPackageRepository repository)
    {
        _filePath = filePath;
        _jsonFileHistory = jsonFileHistory;
        _lockFileHistory = lockFileHistory;
        _parser = parser;
        _repository = repository;
    }

    public string FilePath => _filePath;

    public IEnumerable<DateTimeOffset> Dates
    {
        get
        {
            if (_combinedDates == null)
            {
                var combinedDatesList = new List<DateTimeOffset>();
                combinedDatesList.AddRange(_jsonFileHistory.Dates);
                combinedDatesList.AddRange(_lockFileHistory.Dates);
                combinedDatesList.Sort();
                _combinedDates = combinedDatesList;
            }

            return _combinedDates;
        }
    }

    public IManifest ManifestAsOf(DateTimeOffset asOf)
    {
        var sha = _lockFileHistory.ShaAsOf(asOf);
        var packages = _parser.Parse(_lockFileHistory.ContentStreamAsOf(asOf));

        // TODO: Replace all boolean references for matchMode to the enum
        var matchMode = _parser.UsesExactMatches ? VersionMatchMode.Exact : VersionMatchMode.Expression;

        return new Manifest(_repository, packages, matchMode, sha, _filePath);
    }
}
