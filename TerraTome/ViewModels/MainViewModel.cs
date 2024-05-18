using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using TerraTome.Domain;
using TerraTome.Services;

namespace TerraTome.ViewModels;

public partial class MainViewModel : ViewModelBase, INotifyPropertyChanged
{
    [ObservableProperty]
    private Project? _project;

    [ObservableProperty]
    private ViewModelEntry _selectedViewModel;

    public MainViewModel(IViewModelFactory viewModelFactory)
    {
        this.Views = new ObservableCollection<ViewModelEntry>(viewModelFactory.Views);
        _selectedViewModel = Views.First();
    }

    public bool IsProjectLoaded => Project != null;

    public ObservableCollection<ViewModelEntry> Views { get; }

    public void CloseProject()
    {
        Project = null;
        OnPropertyChanged(nameof(IsProjectLoaded));
    }

    public void LoadProject(Project project)
    {
        Project = project;
        OnPropertyChanged(nameof(IsProjectLoaded));
    }
}