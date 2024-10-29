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

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    /// <param name="activityTracker">An instance of an activity tracker implementation.</param>
    public MainViewModel(IActivityTracker activityTracker)
    {
        // If the user is inactive for more than 3 minutes, stop tracking.
        var trackingTimeout = TimeSpan.FromMinutes(3);

        var filePath = "./test.json";
        var dataAdapter = new FileDataAdapter(filePath);

        this.TrackingService = new TrackingService(activityTracker, trackingTimeout, dataAdapter);
        this.TrackingService.LoadData();

        // Setup Commands
        this.StartTrackingCommand = ReactiveCommand.Create(this.StartTracking);
        this.StopTrackingCommand = ReactiveCommand.Create(this.StopTracking);
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

    /// <summary>
    /// Gets or sets the record which is selected by the user.
    /// </summary>
    public ApplicationData? SelectedRecord { get; set; }

    /// <summary>
    /// Gets or sets the tracking service.
    /// </summary>
    public TrackingService TrackingService { get; set; }

    /// <summary>
    /// Gets the command to show the settings window.
    /// </summary>
    public ReactiveCommand<Unit, Unit> ShowSettingsCommand { get; }

    /// <summary>
    /// Gets the command to start tracking the user's activity.
    /// </summary>
    public ReactiveCommand<Unit, Unit> StartTrackingCommand { get; }

     /// <summary>
     /// Gets the command to stop tracking the user's activity.
     /// </summary>
    public ReactiveCommand<Unit, Unit> StopTrackingCommand { get; }

    /// <summary>
    /// Gets the command to remove the selected record.
    /// </summary>
    public ReactiveCommand<Unit, Unit> RemoveSelectedCommand { get; }

    /// <summary>
    /// Starts tracking the user's activity.
    /// </summary>
    public void StartTracking()
    {
        // Start tracking at 10 second intervals.
        this.TrackingService.StartTracking(TimeSpan.FromSeconds(1));
        this.BoldTitle = $"{DefaultTitle} (TRACKING)";
    }

    /// <summary>
    /// Stops tracking the user's activity.
    /// </summary>
    public void StopTracking()
    {
        this.TrackingService.StopTracking();
        this.BoldTitle = DefaultTitle;
    }

    /// <summary>
    /// Removes the selected record from the list.
    /// </summary>
    public void RemoveSelectedRecord()
    {
        if (this.SelectedRecord == null)
        {
            return;
        }

        this.TrackingService.TrackedApplications.Remove(this.SelectedRecord);
    }
}
