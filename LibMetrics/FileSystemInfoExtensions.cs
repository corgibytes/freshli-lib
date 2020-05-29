using System.IO;
using System.Diagnostics;

namespace LibMetrics
{
  public static class FileSystemInfoExtensions
  {
    // source: https://stackoverflow.com/a/648055/243215
    // author: Vitaliy Ulantikov
    // modifications: does not descend into symbolic link directories
    public static void DeleteReadOnly(this FileSystemInfo fileSystemInfo)
    {
      if (!fileSystemInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
      {
        var directoryInfo = fileSystemInfo as DirectoryInfo;
        if (directoryInfo != null)
        {
          var options = new EnumerationOptions { AttributesToSkip = FileAttributes.System };
          foreach (FileSystemInfo childInfo in directoryInfo.GetFileSystemInfos("*", options))
          {
            childInfo.DeleteReadOnly();
          }
        }
      }

      fileSystemInfo.Attributes = FileAttributes.Normal;
      fileSystemInfo.Delete();
    }
  }
}
