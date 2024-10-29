using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using WindowWatchApp.Avalonia.ViewModels;

namespace WindowWatchApp.Avalonia.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        this.InitializeComponent();
        this.AttachedToVisualTree += this.OnAttachedToVisualTree;
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        // Get the window and subscribe to the closing event
        var window = this.GetVisualRoot() as Window;
        if (window != null)
        {
            window.Closing += this.OnWindowClosing;
        }
    }

    private void OnWindowClosing(object? sender, CancelEventArgs e)
    {
        // Save the data when the window is closing
        var vm = this.DataContext as MainViewModel;

        if (vm != null)
        {
            vm.TrackingService.SaveData();
        }
    }
}
