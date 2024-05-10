using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using TerraTome.Domain;

namespace TerraTome.ViewModels;

public partial class MainViewModel : ViewModelBase, INotifyPropertyChanged
{
    [ObservableProperty]
    private Project? _project;

    public MainViewModel()
    {
    }

    public string Greeting => "Welcome to Avalonia!";
    public bool IsProjectLoaded => Project != null;
    public bool ProjectIsDirty => Project?.IsDirty ?? false;

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