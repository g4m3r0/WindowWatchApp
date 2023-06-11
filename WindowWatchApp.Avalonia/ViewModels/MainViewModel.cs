namespace WindowWatchApp.Avalonia.ViewModels;

using global::Avalonia.Controls;
using global::Avalonia.Data;
using global::Avalonia.Threading;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using WindowWatchApp.Common;
using WindowWatchApp.Common.Models;
using WindowWatchApp.Common.Windows;

public class MainViewModel : ViewModelBase
{
    public MainViewModel(IActivityTracker activityTracker)
    {
        // If the user is inactive for more than 3 minutes, stop tracking.
        var trackingTimeout = TimeSpan.FromMinutes(3);
        this.TrackingService = new TrackingService(activityTracker, trackingTimeout);

        // Setup Commands
        this.StartTrackingCommand = ReactiveCommand.CreateFromTask(this.StartTracking);
    }

    public ObservableCollection<ApplicationData> Test { get; set; }

    // for design time
    public MainViewModel()
    {
        var trackingTimeout = TimeSpan.FromMinutes(3);
        var activityTracker = new WindowsActivityTracker();
        this.TrackingService = new TrackingService(activityTracker, trackingTimeout);
        this.TrackingService.TrackedApplications.Add(new ApplicationData() { ProcessName = "Test", TrackedTime = TimeSpan.FromSeconds(10)});
    }

    public string BoldTitle => "WINDOW WATCH APP";

    public TrackingService TrackingService { get; set; }

    public ReactiveCommand<Unit, Unit> StartTrackingCommand { get; }

    public async Task StartTracking()
    {
        // Start tracking at 10 second intervals.
        this.TrackingService.StartTracking(TimeSpan.FromSeconds(1));
    }
}
