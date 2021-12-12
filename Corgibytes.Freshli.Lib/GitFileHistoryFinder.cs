using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace Corgibytes.Freshli.Lib
{
    // TODO: I'm not sure happy with the way this design has turned out. I'd like for the *Finder classes to be stateless,
    // but provide access to something that carries state. So in the case of a "file history", what we really need is for
    // the "finder" to recognize from a project path (or a remote url) that it knows how to hand back an instance of something
    // that can read the history for any file or files that exist at that location. Perhaps that thing colud be thought of as
    // "file history source"? A "file history source" would be responsible for navigating the underlying data store, such as
    // a git repository or a file system or http service or anything with files, and searching it for the presence of files
    // that might be manifest files. The "file history source" would then be responsible for handing back a "file history" for
    // any single file that it's asked to retrieve. So the finder's job would just be, "can I work with this?" and the "file
    // history source" would know how to actually go and get the information and it would provide that information in the
    // form of "file history" instances.
    public class GitFileHistoryFinder : IFileHistoryFinder
    {
        private Dictionary<string, string> _cloneLocations =
          new Dictionary<string, string>();

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
                Repository.Clone(projectRootPath, cloneLocation);
                _cloneLocations[projectRootPath] = cloneLocation;
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
            return Path.Combine(
              Path.GetTempPath(),
              Guid.NewGuid().ToString()
            );
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

        public IFileHistory FileHistoryOf(
          string projectRootPath,
          string targetFile
        )
        {
            return new GitFileHistory(NormalizeLocation(projectRootPath), targetFile);
        }

        public bool Exists(string projectRootPath, string filePath)
        {
            string clonedProjectRoot = NormalizeLocation(projectRootPath);
            return Directory.GetFiles(clonedProjectRoot, filePath).Any();
        }

        public string ReadAllText(string projectRootPath, string filePath)
        {
            string clonedProjectRoot = NormalizeLocation(projectRootPath);
            return File.ReadAllText(Path.Combine(clonedProjectRoot, filePath));
        }

        public string[] GetManifestFilenames(
          string projectRootPath,
          string pattern
        )
        {
            string clonedProjectRoot = NormalizeLocation(projectRootPath);
            return Directory.GetFiles(clonedProjectRoot,
                                pattern,
                                SearchOption.AllDirectories)
                                .Select(f => Path.GetRelativePath(clonedProjectRoot, f))
                                .ToArray();
        }

        public IFileHistorySource HistorySourceFor(string locator)
        {
            // TODO: Is there any value in caching these?
            return new GitFileHistorySource(locator);
        }
    }
}
