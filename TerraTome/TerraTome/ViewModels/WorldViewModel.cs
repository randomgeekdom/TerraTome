using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using TerraTome.Domain;
using TerraTome.Domain.Dtos;

namespace TerraTome.ViewModels;

public partial class WorldViewModel : ViewModelBase
{
    [ObservableProperty] private TerraTomeBasics _project;

    public ObservableCollection<TextAttributeEntry> TextAttributeEntries { get; } = new();
    public ObservableCollection<NumericAttributeEntry> NumericAttributeEntries { get; } = new();

    public bool IsTextAttributesEmpty => TextAttributeEntries.Count == 0;
    public bool IsTextAttributesNotEmpty => TextAttributeEntries.Count > 0;
    public bool IsNumericAttributesEmpty => NumericAttributeEntries.Count == 0;
    public bool IsNumericAttributesNotEmpty => NumericAttributeEntries.Count > 0;

    public WorldViewModel(TerraTomeProjectDto projectDto)
    {
        _project = new TerraTomeBasics();
        _project.FromDto(projectDto);

        foreach (var kv in _project.TextAttributes)
        {
            TextAttributeEntries.Add(new TextAttributeEntry(kv.Key, kv.Value));
        }
        foreach (var kv in _project.NumericAttributes)
        {
            NumericAttributeEntries.Add(new NumericAttributeEntry(kv.Key, kv.Value));
        }

        TextAttributeEntries.CollectionChanged += Attributes_CollectionChanged;
        NumericAttributeEntries.CollectionChanged += Attributes_CollectionChanged;
    }

    private void Attributes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(IsTextAttributesEmpty));
        OnPropertyChanged(nameof(IsTextAttributesNotEmpty));
        OnPropertyChanged(nameof(IsNumericAttributesEmpty));
        OnPropertyChanged(nameof(IsNumericAttributesNotEmpty));
    }

    public string MonetaryUnit
    {
        get => Project.MonetaryUnit;
        set
        {
            Project.SetMonetaryUnit(value);
            OnPropertyChanged(nameof(MonetaryUnit));
        }
    }

    public Dictionary<string, string> TextAttributes
    {
        get => Project.TextAttributes;
    }

    public Dictionary<string, double> NumericAttributes
    {
        get => Project.NumericAttributes;
    }

    public override string Name => "Basics";

    public string Notes
    {
        get => Project.Notes;
        set
        {
            Project.SetNotes(value);
            OnPropertyChanged(nameof(Notes));
        }
    }

    public string TimelineUnit
    {
        get => Project.TimelineUnit;
        set
        {
            Project.SetTimelineUnit(value);
            OnPropertyChanged(nameof(TimelineUnit));
        }
    }

    public string WorldName
    {
        get => Project.Name;
        set
        {
            Project.SetName(value);
            OnPropertyChanged(nameof(Name));
        }
    }

    public override void MapToDto(TerraTomeProjectDto project)
    {
        Project.ToDto(project);

        var attributes = new List<AttributeDto> { };
        foreach (var entries in TextAttributeEntries)
        {
            attributes.Add(new AttributeDto { Name = entries.Name, Type = typeof(string), Value = entries.Value });
        }
        foreach (var entries in NumericAttributeEntries)
        {
            attributes.Add(new AttributeDto { Name = entries.Name, Type = typeof(double), Value = entries.Value });
        }
        project.Attributes = [.. attributes];
    }

    public void ResetDirty()
    {
        this.IsDirty = false;
    }
}