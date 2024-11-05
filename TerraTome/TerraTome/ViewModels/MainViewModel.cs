using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using TerraTome.Domain;

namespace TerraTome.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	[ObservableProperty] private string _greeting = "Welcome to Avalonia!";
	[ObservableProperty] private TerraTomeProject? _terraTomeProject;
	[ObservableProperty] private ViewModelBase? _currentViewModel;
	[ObservableProperty] private ObservableCollection<ViewModelBase> _viewModels = [];
    
	public bool IsProjectLoaded => TerraTomeProject is not null;
	public bool IsProjectNotLoaded => TerraTomeProject is null;


	public void SetProject(TerraTomeProject project)
	{
		TerraTomeProject = project;
        
		ViewModels.Clear();
		ViewModels.Add(new WorldViewModel(project));
		ViewModels.Add(new TimelineViewModel());
        
		OnPropertyChanged(nameof(IsProjectLoaded));
		OnPropertyChanged(nameof(IsProjectNotLoaded));
	}

	public override string Name => "Name";
}