namespace WindowWatchApp.Common.DataAdapters;

using System.Collections.ObjectModel;
using WindowWatchApp.Common.Models;

public interface IDataAdapter
{
    ObservableCollection<ApplicationData> Load();

    void Save(ObservableCollection<ApplicationData> data);
}