using TerraTome.Domain.Dtos;

namespace TerraTome.ViewModels;

public partial class TimelineViewModel : ViewModelBase
{
	public override string Name => "Timeline";

    public override void MapToDto(TerraTomeProjectDto project)
    {
        // No-op for now
    }
}