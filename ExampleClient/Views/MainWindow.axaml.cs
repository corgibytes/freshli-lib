using System;
using System.Diagnostics;
using System.Reactive;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ExampleClient.ViewModels;
using ReactiveUI;
using Avalonia.Reactive;


namespace ExampleClient.Views {
  public class MainWindow : ReactiveWindow<MainWindowViewModel> {

    public MainWindow() {
      InitializeComponent();
#if DEBUG
      this.AttachDevTools();
#endif

      // Close the Window when the close command is triggered.
      this.WhenActivated(block: d => {
          Debug.Assert(ViewModel != null, nameof(ViewModel) + " != null");
          d(ViewModel.CloseCommand.Subscribe(onNext: (unit) => Close()));
        }
      );
    }

    private void InitializeComponent() {
      AvaloniaXamlLoader.Load(this);
    }
  }
}
