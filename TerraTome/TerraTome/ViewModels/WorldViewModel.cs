using CommunityToolkit.Mvvm.ComponentModel;
using TerraTome.Domain;
using TerraTome.Domain.Dtos;

namespace TerraTome.ViewModels;

public partial class WorldViewModel : ViewModelBase
{
	public override string Name => "Basics";
	[ObservableProperty] private TerraTomeBasics _project;

    public WorldViewModel(TerraTomeProjectDto projectDto)
    {
		_project = new TerraTomeBasics();
		_project.FromDto(projectDto);
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
	
	public string TimelineUnit
	{
		get => Project.TimelineUnit;
		set
		{
			Project.SetTimelineUnit(value);
			OnPropertyChanged(nameof(TimelineUnit));
		}
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

	public string Notes
	{
		get => Project.Notes;
		set
		{
			Project.SetNotes(value);
			OnPropertyChanged(nameof(Notes));
		}
	}
}