namespace WindowWatchApp.Common
{
    public interface IActivityTracker
    {
        TimeSpan InactivityPeriod { get; }
        string GetActiveApplication();
    }
}
