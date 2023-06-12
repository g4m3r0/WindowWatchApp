namespace WindowWatchApp.Avalonia.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using global::Avalonia.Controls;
using global::Avalonia.Data;
using global::Avalonia.Threading;
using ReactiveUI;
using WindowWatchApp.Common;
using WindowWatchApp.Common.DataAdapters;
using WindowWatchApp.Common.Models;
using WindowWatchApp.Common.Windows;

public class MainViewModel : ViewModelBase
{
    private const string DefaultTitle = "WINDOW WATCH APP";

    private string boldTitle = DefaultTitle;

    public MainViewModel(IActivityTracker activityTracker)
    {
        // If the user is inactive for more than 3 minutes, stop tracking.
        var trackingTimeout = TimeSpan.FromMinutes(3);

        var filePath = "./test.json";
        var dataAdapter = new FileDataAdapter(filePath);

        this.TrackingService = new TrackingService(activityTracker, trackingTimeout, dataAdapter);
        this.TrackingService.LoadData();

        // Setup Commands
        this.StartTrackingCommand = ReactiveCommand.CreateFromTask(this.StartTracking);
        this.StopTrackingCommand = ReactiveCommand.CreateFromTask(this.StopTracking);
        this.RemoveSelectedCommand = ReactiveCommand.Create(this.RemoveSelectedRecord);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    public MainViewModel()
    {
        // for design time
        var activityTracker = new WindowsActivityTracker();
        var trackingTimeout = TimeSpan.FromMinutes(3);

        var filePath = "./test.json";
        var dataAdapter = new FileDataAdapter(filePath);

        this.TrackingService = new TrackingService(activityTracker, trackingTimeout, dataAdapter);
        this.TrackingService.LoadData();

        this.TrackingService.TrackedApplications.Add(new ApplicationData() { ProcessName = "Test", TrackedTime = TimeSpan.FromSeconds(10) });
    }

    /// <summary>
    /// Gets or sets the bold application title.
    /// </summary>
    public string BoldTitle
    {
        get => this.boldTitle;
        set => this.RaiseAndSetIfChanged(ref this.boldTitle, value);
    }

    public TrackingService TrackingService { get; set; }

    public ReactiveCommand<Unit, Unit> ShowSettingsCommand { get; }

    public ReactiveCommand<Unit, Unit> StartTrackingCommand { get; }

    public ReactiveCommand<Unit, Unit> StopTrackingCommand { get; }

    public ReactiveCommand<Unit, Unit> RemoveSelectedCommand { get; }

    public async Task StartTracking()
    {
        // Start tracking at 10 second intervals.
        this.TrackingService.StartTracking(TimeSpan.FromSeconds(1));

        this.BoldTitle = $"{DefaultTitle} (TRACKING)";
    }

    public async Task StopTracking()
    {
        this.TrackingService.StopTracking();
        this.BoldTitle = DefaultTitle;
    }

    public void RemoveSelectedRecord()
    {
        // TODO: Implement

    }
}
