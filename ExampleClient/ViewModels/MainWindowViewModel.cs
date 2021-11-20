using System;
using System.Reactive;
using Corgibytes.Freshli.Lib;
using NLog;
using ReactiveUI;

namespace ExampleClient.ViewModels
{
    /// <summary>
    /// Allow developers to run Freshli Lib commands.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The path to the Git repo to run Freshli against.
        /// </summary>
        public string GitPath { get; set; }

        /// <summary>
        /// Show the results from Freshli.
        /// </summary>
        public string Results
        {
            get => _results;
            set => this.RaiseAndSetIfChanged(ref _results, value);
        }
        private string _results;

        /// <summary>
        /// Close the window.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CloseCommand { get; }

        /// <summary>
        /// Run Freshli.
        /// </summary>
        public ReactiveCommand<Unit, Unit> OnRunFreshliClick { get; }

        /// <summary>
        /// Initialize the view model.
        /// </summary>
        public MainWindowViewModel()
        {
            _results = "";

            GitPath = "";
            Results = "";

            OnRunFreshliClick = ReactiveCommand.Create(RunFreshli);
            CloseCommand = ReactiveCommand.Create(execute: () => { });
        }

        /// <summary>
        /// Run Freshli and show the results to the user.
        /// </summary>
        private void RunFreshli()
        {
            Results = $"Starting Freshli run for '{GitPath}'{Environment.NewLine}";

            var manifestFinderRegistry = new ManifestFinderRegistry();

            var loader = new ManifestFinderRegistryLoader(LogManager.GetLogger(nameof(ManifestFinderRegistryLoader)));
            loader.RegisterAll(manifestFinderRegistry);

            var fileHistoryFinderRegistry = new FileHistoryFinderRegistry();
            fileHistoryFinderRegistry.Register<GitFileHistoryFinder>();
            fileHistoryFinderRegistry.Register<LocalFileHistoryFinder>();

            var runner = new Runner(manifestFinderRegistry, fileHistoryFinderRegistry);
            var metricResults = runner.Run(GitPath);

            foreach (var mr in metricResults)
            {
                Results += mr.ToString();
            }

            Results += $"Finished!{Environment.NewLine}";
        }
    }
}
