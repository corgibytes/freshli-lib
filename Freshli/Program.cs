using System;
using System.IO;
using Freshli.Languages.Php;
using Freshli.Languages.Ruby;

namespace Freshli
{
    class Program
    {
        static void Main(string[] args)
        {
          ManifestFinder.Register<RubyBundlerManifestFinder>();
          ManifestFinder.Register<PhpComposerManifestFinder>();

          FileHistoryFinder.Register<GitFileHistoryFinder>();

          var runner = new Runner();
          var directory = SafelyGetFullPath(args[0]);

          Console.Error.WriteLine($"Collecting data for {directory}");

          var results = runner.Run(args[0]);

          var formatter = new OutputFormatter(Console.Out);
          formatter.Write(results);
        }

        private static string SafelyGetFullPath(string path)
        {
          var result = path;
          if (File.Exists(path))
          {
            result = Path.GetFullPath(path);
          }

          return result;
        }
    }
}
