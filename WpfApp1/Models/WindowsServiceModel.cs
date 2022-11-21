namespace WpfApp1.Models;

public class WindowsServiceModel
{
    public WindowsServiceModel(string name, string displayName, string type, string status, string startType) {
        Name = name;
        DisplayName = displayName;
        Type = type;
        State = status;
        StartType = startType;
    }

    public string DisplayName { get; }
    public string Type { get; }
    public string Name { get; }
    public string State { get; }
    public string StartType { get; }

}
