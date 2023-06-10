namespace WindowWatchApp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Timers;
    using WindowWatchApp.Common;

    /// <summary>
    /// Provides a service for tracking user activity based on active application.
    /// </summary>
    public class TrackingService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingService"/> class.
        /// </summary>
        /// <param name="activityTracker">The IActivityTracker implementation to use.</param>
        /// <param name="activityTimeout">The maximum allowed period of user inactivity.</param>
        public TrackingService(IActivityTracker activityTracker, TimeSpan activityTimeout)
        {
            this.ActivityTracker = activityTracker;
            this.ActivityTimeout = activityTimeout;
        }

        /// <summary>
        /// Gets or sets a dictionary that tracks the time spent per application.
        /// </summary>
        public Dictionary<string, TimeSpan> TrackedTime { get; set; } = new();

        /// <summary>
        /// Gets or sets the maximum allowed period of user inactivity. If the user is inactive longer than this, the service stops tracking.
        /// </summary>
        public TimeSpan ActivityTimeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the service is actively tracking.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the tracking interval. This indicates the frequency with which the service checks for active applications.
        /// </summary>
        private TimeSpan TrackingInterval { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Gets or sets the timer for tracking the active application.
        /// </summary>
        private Timer? TrackTimer { get; set; }

        /// <summary>
        /// Gets the IActivityTracker implementation that the service uses to track activity.
        /// </summary>
        private IActivityTracker ActivityTracker { get; init; }

        /// <summary>
        /// Starts tracking activity at a specified interval.
        /// </summary>
        /// <param name="trackingInterval">The interval at which to track activity.</param>
        public void StartTracking(TimeSpan trackingInterval)
        {
            this.TrackingInterval = trackingInterval;
            this.TrackTimer = new Timer(this.TrackingInterval.TotalMilliseconds);
            this.TrackTimer.Elapsed += this.TrackingTimerElapsed;
            this.TrackTimer.Start();
        }

        /// <summary>
        /// Stops tracking activity.
        /// </summary>
        public void StopTracking()
        {
            if (this.TrackTimer == null)
            {
                return;
            }

            this.TrackTimer.Stop();
        }

        /// <summary>
        /// Handles the Elapsed event of the tracking timer. Checks for user inactivity and tracks active application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An ElapsedEventArgs that contains the event data.</param>
        private void TrackingTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (this.ActivityTimeout <= this.ActivityTracker.InactivityPeriod)
            {
                return;
            }

            this.TrackTime(this.ActivityTracker.GetActiveApplication());
        }

        /// <summary>
        /// Tracks the time spent on a given process.
        /// </summary>
        /// <param name="processName">Name of the process to track.</param>
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
