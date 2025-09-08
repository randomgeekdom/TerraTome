using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TerraTome.Constants;
using TerraTome.Domain.Dtos;
using TerraTome.Events;
using TerraTome.Services;

namespace TerraTome.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase? _currentViewModel;
    [ObservableProperty] private string _greeting = "Welcome to Avalonia!";
    [ObservableProperty] private TerraTomeProjectDto? _project;
    [ObservableProperty] private Uri? _projectFilePath;
    [ObservableProperty] private ObservableCollection<ViewModelBase> _viewModels = [];
    public ICommand CreateCommand => new AsyncRelayCommand(CreateAsync);
    public bool IsProjectLoaded => Project is not null;
    public bool IsProjectNotLoaded => Project is null;
    public ICommand LoadCommand => new AsyncRelayCommand(LoadAsync);
    public override string Name => "Name";
    public ICommand SaveAsCommand => new AsyncRelayCommand(SaveAsAsync);
    public ICommand SaveCommand => new AsyncRelayCommand(SaveAsync);
    public ObservableCollection<ViewModelBase> VisibleViewModels => new(ViewModels.Where(vm => vm.IsVisible));

    public override void MapToDto(TerraTomeProjectDto project)
    {
        project.OpenTabs = [.. ViewModels.Where(vm => vm.IsVisible).Select(vm => vm.Name)];
        foreach (var vm in ViewModels)
        {
            vm.MapToDto(project);
        }
    }

    public void SetProject(TerraTomeProjectDto project, Uri filePath)
    {
        this.ProjectFilePath = filePath;
        this.Project = project;

        ViewModels = [];

        if (project.OpenTabs.Contains(TabNames.Basics))
        {
            ViewModels.Add(new WorldViewModel(project));
        }

        if (project.OpenTabs.Contains(TabNames.Timeline))
        {
            ViewModels.Add(new TimelineViewModel());
        }

        foreach (var vm in ViewModels)
        {
            vm.TabCloseRequested += OnTabCloseRequested;
        }

        OnPropertyChanged(nameof(IsProjectLoaded));
        OnPropertyChanged(nameof(IsProjectNotLoaded));
        OnPropertyChanged(nameof(VisibleViewModels));
    }

    private async Task CreateAsync()
    {
        var topLevel = ApplicationService.GetTopLevel();
        if (topLevel is null) return;

        var file = await ApplicationService.ShowSaveFileDialog(topLevel, "Save Project");
        if (file == null) return;

        this.SetProject(new TerraTomeProjectDto(), file.Path);
        await ApplicationService.SaveProjectAsync(file, this);
    }

    private async Task LoadAsync()
    {
        var topLevel = ApplicationService.GetTopLevel();
        if (topLevel is null) return;

        var files = await ApplicationService.ShowOpenFileDialog(topLevel, "Load Project");
        if (files.Count < 1) return;

        var projectDto = await ApplicationService.DeserializeProjectDtoAsync(files[0]);
        this.SetProject(projectDto!, files[0].Path);
    }

    private void OnTabCloseRequested(object? sender, EventArgs e)
    {
        var tabClosedArgs = e as TabCloseEventArgs;

        //todo: handle saving
        if (tabClosedArgs!.IsSaving)
        {
            this.SaveCommand.Execute(null);
        }

        OnPropertyChanged(nameof(VisibleViewModels));
        if (CurrentViewModel is not null && !CurrentViewModel.IsVisible)
        {
            CurrentViewModel = VisibleViewModels.FirstOrDefault();
        }
    }

    private async Task SaveAsAsync()
    {
        var topLevel = ApplicationService.GetTopLevel();
        if (topLevel is null) return;

        var file = await ApplicationService.ShowSaveFileDialog(topLevel, "Save Project Copy");
        if (file == null) return;
        this.ProjectFilePath = file.Path;
        await ApplicationService.SaveProjectAsync(file, this);
    }

    private async Task SaveAsync()
    {
        var topLevel = ApplicationService.GetTopLevel();
        if (topLevel is null) return;

        var file = await topLevel.StorageProvider.TryGetFileFromPathAsync(filePath: this.ProjectFilePath!);
        if (file == null) throw new FileNotFoundException();
        await ApplicationService.SaveProjectAsync(file, this);
    }
}