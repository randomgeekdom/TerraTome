using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TerraTome.Domain;
using TerraTome.Domain.Dtos;

namespace TerraTome.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private string _greeting = "Welcome to Avalonia!";
    [ObservableProperty] private TerraTomeProjectSettings? _terraTomeProjectSettings;
    [ObservableProperty] private ViewModelBase? _currentViewModel;
    [ObservableProperty] private ObservableCollection<ViewModelBase> _viewModels = [];

    public ObservableCollection<ViewModelBase> VisibleViewModels => new(ViewModels.Where(vm => vm.IsVisible));

    public bool IsProjectLoaded => TerraTomeProjectSettings is not null;
    public bool IsProjectNotLoaded => TerraTomeProjectSettings is null;

    private void OnTabCloseRequested(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(VisibleViewModels));
        if (CurrentViewModel is not null && !CurrentViewModel.IsVisible)
        {
            CurrentViewModel = VisibleViewModels.FirstOrDefault();
        }
    }

    public void SetProject(TerraTomeProjectDto project, Uri filePath)
    {
        TerraTomeProjectSettings = TerraTomeProjectSettings.TryCreate(filePath);

        ViewModels = new ObservableCollection<ViewModelBase> { new WorldViewModel(project), new TimelineViewModel { IsVisible = false } };
        foreach (var vm in ViewModels)
        {
            vm.TabCloseRequested += OnTabCloseRequested;
        }

        OnPropertyChanged(nameof(IsProjectLoaded));
        OnPropertyChanged(nameof(IsProjectNotLoaded));
        OnPropertyChanged(nameof(VisibleViewModels));
    }

    public override string Name => "Name";
}