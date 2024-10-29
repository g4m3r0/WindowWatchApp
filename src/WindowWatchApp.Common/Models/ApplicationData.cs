using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WindowWatchApp.Common.Models;

public class ApplicationData : INotifyPropertyChanged
{
    private TimeSpan trackedTime;

    public string ProcessName { get; set; }

    public TimeSpan TrackedTime
    {
        get
        {
            return this.trackedTime;
        }

        set
        {
            if (this.trackedTime != value)
            {
                this.trackedTime = value;
                this.OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
