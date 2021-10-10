using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace Corgibytes.Freshli.Lib
{
    public class GitFileHistoryFinder : IFileHistoryFinder
    {
        private Dictionary<string, string> _cloneLocations =
          new Dictionary<string, string>();

        // TODO: Make an async version of this method
        private string NormalizeLocation(string projectRootPath)
        {
            if (Repository.IsValid(projectRootPath))
            {
                return projectRootPath;
            }

            if (_cloneLocations.ContainsKey(projectRootPath))
            {
                return _cloneLocations[projectRootPath];
            }

            if (IsCloneable(projectRootPath))
            {
                var cloneLocation = GenerateTempCloneLocation();
                // TODO: Check to see if there's a way to call this method in an async way
                Repository.Clone(projectRootPath, cloneLocation);
                _cloneLocations[projectRootPath] = cloneLocation;
                return cloneLocation;
            }

            return projectRootPath;
        }

        public bool DoesPathContainHistorySource(string projectRootPath)
        {
            // TODO: Check to see if there's a way to call this method in an async way
            bool result = Repository.IsValid(projectRootPath);
            if (!result)
            {
                result = IsCloneable(projectRootPath);
            }

            return result;
        }

        private string GenerateTempCloneLocation()
        {
            return Path.Combine(
              Path.GetTempPath(),
              Guid.NewGuid().ToString()
            );
        }

        // TODO: Create an async version of this method
        private bool IsCloneable(string url)
        {
            var result = true;
            var options = new CloneOptions { Checkout = false };

            string tempFolder = GenerateTempCloneLocation();

            try
            {
                // TODO: See if there's an asnyc version of this method and call it
                Repository.Clone(url, tempFolder, options);
            }
            catch (NotFoundException)
            {
                result = false;
            }

            // TODO: Do this in an async way
            if (Directory.Exists(tempFolder))
            {
                new DirectoryInfo(tempFolder).DeleteReadOnly();
            }

            return result;
        }

        // TODO: Create an async version of this method
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

        public IFileHistory FileHistoryOf(string projectRootPath, string targetFile)
        {
            return new GitFileHistory(NormalizeLocation(projectRootPath), targetFile);
        }

        public bool Exists(string projectRootPath, string filePath)
        {
            string clonedProjectRoot = NormalizeLocation(projectRootPath);
            // TODO: Call this in an async way
            return Directory.GetFiles(clonedProjectRoot, filePath).Any();
        }

        public string ReadAllText(string projectRootPath, string filePath)
        {
            string clonedProjectRoot = NormalizeLocation(projectRootPath);
            // TODO: Call this in an async way
            return File.ReadAllText(Path.Combine(clonedProjectRoot, filePath));
        }

        public string[] GetManifestFilenames(string projectRootPath, string pattern)
        {
            string clonedProjectRoot = NormalizeLocation(projectRootPath);
            // TODO: Call this in an async way
            return Directory
                .GetFiles(clonedProjectRoot, pattern, SearchOption.AllDirectories)
                .Select(f => Path.GetRelativePath(clonedProjectRoot, f))
                .ToArray();
        }
    }
}
