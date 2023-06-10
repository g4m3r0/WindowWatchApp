namespace WindowWatchApp.Console
{
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System;
    using WindowWatchApp.Common.Windows;

    class Program
    {
        static async Task Main(string[] args)
        {
            Task.Run(RunStuff);
            Console.ReadLine();
        }

        public static async Task RunStuff()
        {
            try
            {
                var activityTracker = new WindowsActivityTracker();

                var trackingTimeout = TimeSpan.FromMinutes(3);
                var trackingService = new TrackingService(activityTracker, trackingTimeout);

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
}