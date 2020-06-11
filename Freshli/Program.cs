using System;
using System.IO;
using System.Linq;
using Freshli.Languages.Php;
using Freshli.Languages.Python;
using Freshli.Languages.Ruby;
using NLog;

namespace Freshli
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
          logger.Info($"Main({args})");
          
          ManifestFinder.Register<RubyBundlerManifestFinder>();
          ManifestFinder.Register<PhpComposerManifestFinder>();
          ManifestFinder.Register<PipRequirementsTxtManifestFinder>();

          FileHistoryFinder.Register<GitFileHistoryFinder>();

          var runner = new Runner();
          var directory = SafelyGetFullPath(args[0]);

          logger.Info($"Collecting data for {directory}");

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
