using CommunityToolkit.Mvvm.ComponentModel;

namespace TerraTome.ViewModels;

public partial class TextAttributeEntry : ObservableObject
{
    public string Name { get; }

    [ObservableProperty]
    private string _value = string.Empty;

    public TextAttributeEntry(string name, string value)
    {
        Name = name;
        _value = value;
    }
}

public partial class NumericAttributeEntry : ObservableObject
{
    public string Name { get; }

    [ObservableProperty]
    private double _value;

    public NumericAttributeEntry(string name, double value)
    {
        Name = name;
        _value = value;
    }
}