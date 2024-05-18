using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using TerraTome.Domain;

namespace TerraTome.ViewModels;

public partial class MainViewModel : ViewModelBase, INotifyPropertyChanged
{
    [ObservableProperty]
    private Project? _project;

    [ObservableProperty]
    private AggregateViewModel _selectedViewModel;

    public MainViewModel()
    {
        _selectedViewModel = Views.First();
    }

    public string Greeting => "Welcome to Avalonia!";
    public bool IsProjectLoaded => Project != null;

    public ObservableCollection<AggregateViewModel> Views { get; } =
    [
        new LocalesViewModel(),
        new EventsViewModel(),
    ];

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