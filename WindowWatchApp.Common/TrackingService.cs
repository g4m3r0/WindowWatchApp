namespace WindowWatchApp.Console
{
    using System;
    using System.Collections.Generic;
    using System.Timers;
    using WindowWatchApp.Common;

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
