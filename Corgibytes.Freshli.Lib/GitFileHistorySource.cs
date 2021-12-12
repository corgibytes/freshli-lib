using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace Corgibytes.Freshli.Lib;

public class GitFileHistorySource : IFileHistorySource
{
    private string _locator;
    private string _cloneLocation;

    public GitFileHistorySource(string locator)
    {
        _locator = locator;
    }

    public bool ContainsFileHistory => throw new System.NotImplementedException();

    private string NormalizeLocation(string projectRootPath)
    {
        if (Repository.IsValid(projectRootPath))
        {
            return projectRootPath;
        }

        if (_cloneLocation != null)
        {
            return _cloneLocation;
        }

        if (IsCloneable(projectRootPath))
        {
            var cloneLocation = GenerateTempCloneLocation();
            Repository.Clone(projectRootPath, cloneLocation);
            _cloneLocation = cloneLocation;
            return cloneLocation;
        }

        return projectRootPath;
    }

    public bool DoesPathContainHistorySource(string projectRootPath)
    {
        bool result = Repository.IsValid(projectRootPath);
        if (!result)
        {
            result = IsCloneable(projectRootPath);
        }

        return result;
    }

    private string GenerateTempCloneLocation()
    {
        return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
    }

    private bool IsCloneable(string url)
    {
        var result = true;
        var options = new CloneOptions { Checkout = false };

        string tempFolder = GenerateTempCloneLocation();

        try
        {
            Repository.Clone(url, tempFolder, options);
        }
        catch (NotFoundException)
        {
            result = false;
        }

        if (Directory.Exists(tempFolder))
        {
            new DirectoryInfo(tempFolder).DeleteReadOnly();
        }

        return result;
    }

    private void RecursivelyClearReadOnlyAttribute(string path)
    {
        foreach (var childDirectory in Directory.EnumerateDirectories(path))
        {
            RecursivelyClearReadOnlyAttribute(childDirectory);
        }

        foreach (var childFile in Directory.EnumerateFiles(path))
        {
            File.SetAttributes(childFile, FileAttributes.Normal);
        }
    }

    public IFileHistory FileHistoryOf(string targetFile)
    {
        return new GitFileHistory(NormalizeLocation(_locator), targetFile);
    }

    public bool Exists(string filePath)
    {
        string clonedProjectRoot = NormalizeLocation(_locator);
        return Directory.GetFiles(clonedProjectRoot, filePath).Any();
    }

    public string ReadAllText(string filePath)
    {
        string clonedProjectRoot = NormalizeLocation(_locator);
        return File.ReadAllText(Path.Combine(clonedProjectRoot, filePath));
    }

    public string[] GetManifestFilenames(string pattern)
    {
        string clonedProjectRoot = NormalizeLocation(_locator);
        return Directory
            .GetFiles(clonedProjectRoot, pattern, SearchOption.AllDirectories)
            .Select(f => Path.GetRelativePath(clonedProjectRoot, f))
            .ToArray();
    }

}