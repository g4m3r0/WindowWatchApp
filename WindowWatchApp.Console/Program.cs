namespace WindowWatchApp.Console;

using System;
using WindowWatchApp.Common;
using WindowWatchApp.Common.Windows;

public static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args"></param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public static async Task Main(string[] args)
    {
        try
        {
            var activityTracker = new WindowsActivityTracker();

            // If the user is inactive for more than 3 minutes, stop tracking.
            var trackingTimeout = TimeSpan.FromMinutes(3);
            var trackingService = new TrackingService(activityTracker, trackingTimeout);

            // Start tracking at 10 second intervals.
            trackingService.StartTracking(TimeSpan.FromSeconds(10));

            while (true)
            {
                await Task.Delay(1000);
                Console.Clear();
                await Console.Out.WriteLineAsync("Tracked Applications:");

                foreach (var trackedApp in trackingService.TrackedTime)
                {
                    await Console.Out.WriteLineAsync($"{trackedApp.Key}: {trackedApp.Value.TotalSeconds}s");
                }
            }
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
        }
    }
}