using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using TerraTome.Domain;
using TerraTome.Domain.Dtos;

namespace TerraTome.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	[ObservableProperty] private string _greeting = "Welcome to Avalonia!";
	[ObservableProperty] private TerraTomeProjectSettings? _terraTomeProjectSettings;
	[ObservableProperty] private ViewModelBase? _currentViewModel;
	[ObservableProperty] private ObservableCollection<ViewModelBase> _viewModels = [];
    
	public bool IsProjectLoaded => TerraTomeProjectSettings is not null;
	public bool IsProjectNotLoaded => TerraTomeProjectSettings is null;


	public void SetProject(TerraTomeProjectDto project, Uri filePath)
	{
		TerraTomeProjectSettings = TerraTomeProjectSettings.TryCreate(filePath);

		ViewModels = [new WorldViewModel(project), new TimelineViewModel { IsVisible = false }];

		OnPropertyChanged(nameof(IsProjectLoaded));
		OnPropertyChanged(nameof(IsProjectNotLoaded));
	}

	public override string Name => "Name";
}