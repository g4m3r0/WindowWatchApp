namespace WindowWatchApp.Common.DataAdapters;

using System.Collections.ObjectModel;
using System.Text.Json;
using WindowWatchApp.Common.Models;

public class FileDataAdapter : IDataAdapter
{
    public FileDataAdapter(string filePath)
    {
        this.FilePath = filePath;
    }

    private string FilePath { get; set; }

    public ObservableCollection<ApplicationData> Load()
    {
        if (File.Exists(this.FilePath))
        {
            var jsonData = File.ReadAllText(this.FilePath);
            var data = JsonSerializer.Deserialize<ObservableCollection<ApplicationData>>(jsonData);
            return data ?? new ObservableCollection<ApplicationData>();
        }
        else
        {
            return new ObservableCollection<ApplicationData>();
        }
    }

    public void Save(ObservableCollection<ApplicationData> data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true }; // make the json more readable
        var jsonData = JsonSerializer.Serialize(data, options);
        File.WriteAllText(this.FilePath, jsonData);
    }
}
