namespace WindowWatchApp.Avalonia.ViewModels;

using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using WindowWatchApp.Common;
using WindowWatchApp.Common.Models;

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

    // for design time
    public MainViewModel()
    {
        Testtt.Add(new ApplicationData() { ProcessName = "Test", TrackedTime = TimeSpan.FromSeconds(10)});
    }

    public ObservableCollection<ApplicationData> Testtt { get; set; } = new();

    public string BoldTitle => "WINDOW WATCH APP";

    public TrackingService TrackingService { get; set; }

    public ReactiveCommand<Unit, Unit> StartTrackingCommand { get; }

    public async Task StartTracking()
    {
        // Start tracking at 10 second intervals.
        this.TrackingService.StartTracking(TimeSpan.FromSeconds(1));
    }
}
