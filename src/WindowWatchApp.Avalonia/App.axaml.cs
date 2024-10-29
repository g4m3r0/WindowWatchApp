using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using WindowWatchApp.Avalonia.ViewModels;
using WindowWatchApp.Avalonia.Views;
namespace WindowWatchApp.Avalonia;

using WindowWatchApp.Common.Windows;
using WindowWatchApp.Common;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // todo change activity tracker depending on platform
        var activityTracker = new WindowsActivityTracker();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(activityTracker)
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel(activityTracker)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
