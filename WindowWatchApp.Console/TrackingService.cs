namespace WindowWatchApp.Console
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Timers;
    using WindowWatchApp.Common;
    using WindowWatchApp.Common.Windows;

    public class TrackingService
    {
        public Dictionary<string, TimeSpan> TrackedTime { get; set; } = new();
        private TimeSpan TrackingInterval { get; set; } = TimeSpan.FromSeconds(1);

        private Timer TrackTimer { get; set; }
        private Timer ActivityTimer { get; set; }

        public TimeSpan ActivityTimeout { get; set; }

        private IActivityTracker ActivityTracker { get; init; }

        public bool IsActive { get; set; } = true;

        public TrackingService(IActivityTracker activityTracker, TimeSpan activityTimeout)
        {
            this.ActivityTracker = activityTracker;
            this.ActivityTimeout = activityTimeout;
            //this.ActivityTimer.Elapsed += ActivityTimer_Elapsed;
            //this.ActivityTimer.AutoReset = false; // Only trigger once until reset
        }

        public void StartTracking(TimeSpan trackingInterval)
        {
            this.TrackingInterval = trackingInterval;
            this.TrackTimer = new Timer(TrackingInterval.TotalMilliseconds);
            this.TrackTimer.Elapsed += TrackingTimerElapsed;
            this.TrackTimer.Start();
        }

        public void StopTracking()
        {
            this.TrackTimer.Stop();
        }

        //private void ActivityTracker_ActivityOccurred(object sender, EventArgs e)
        //{
        //    // Reset the timer
        //    this.ActivityTimer.Stop();
        //    this.ActivityTimer.Start();

        //    // Set IsActive to true
        //    this.IsActive = true;
        //}

        //private void ActivityTimer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    // No activity occurred within the timeout period, set IsActive to false
        //    this.IsActive = false;
        //}

        private void TrackingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (this.ActivityTimeout <= this.ActivityTracker.InactivityPeriod)
            {
                return;
            }

            this.TrackTime(this.ActivityTracker.GetActiveApplication());
        }

        private void TrackTime(string processName)
        {
            if (this.TrackedTime.ContainsKey(processName))
            {
                this.TrackedTime[processName] += this.TrackingInterval;
            }
            else
            {
                this.TrackedTime.Add(processName, this.TrackingInterval);
            }
        }
    }
}
