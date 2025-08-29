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
    [ObservableProperty] private ViewModelBase? _currentViewModel;
    [ObservableProperty] private ObservableCollection<ViewModelBase> _viewModels = [];
    [ObservableProperty] private TerraTomeProjectDto? _project;
    [ObservableProperty] private Uri? _projectFilePath;

    public ObservableCollection<ViewModelBase> VisibleViewModels => new(ViewModels.Where(vm => vm.IsVisible));

    public bool IsProjectLoaded => Project is not null;
    public bool IsProjectNotLoaded => Project is null;

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
        this.ProjectFilePath = filePath;
        this.Project = project;

        ViewModels = [new WorldViewModel(project), new TimelineViewModel { IsVisible = false }];
        foreach (var vm in ViewModels)
        {
            vm.TabCloseRequested += OnTabCloseRequested;
        }

        OnPropertyChanged(nameof(IsProjectLoaded));
        OnPropertyChanged(nameof(IsProjectNotLoaded));
        OnPropertyChanged(nameof(VisibleViewModels));
    }

    public override void MapToDto(TerraTomeProjectDto project)
    {
        foreach(var vm in ViewModels)
        {
            vm.MapToDto(project);
        }
    }

    public override string Name => "Name";
}