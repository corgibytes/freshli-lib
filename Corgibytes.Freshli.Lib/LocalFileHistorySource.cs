using System;
using System.IO;
using System.Linq;

namespace Corgibytes.Freshli.Lib;

public class LocalFileHistorySource : IFileHistorySource
{
    private string _rootPath;

    public LocalFileHistorySource(string rootPath)
    {
        _rootPath = rootPath;
    }

    public bool ContainsFileHistory => true;

    public IFileHistory FileHistoryOf(string targetFile)
    {
        return new LocalFileHistory(_rootPath, targetFile);
    }

    public bool Exists(string filePath)
    {
        return File.Exists(Path.Combine(_rootPath, filePath));
    }

    public string ReadAllText(string filePath)
    {
        return File.ReadAllText(Path.Combine(_rootPath, filePath));
    }

    public string[] GetManifestFilenames(string pattern)
    {
        return Directory.
            GetFiles(_rootPath, pattern, SearchOption.AllDirectories).Select(f => Path.GetFileName(f)).ToArray();
    }

}

