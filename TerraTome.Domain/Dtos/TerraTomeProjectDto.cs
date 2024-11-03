namespace TerraTome.Domain.Dtos;

public class TerraTomeProjectDto:EntityDto
{
	public string Name { get; set; }
	public string TimelineUnit { get; set; }
	public string MonetaryUnit { get; set; }
	public string Notes { get; set; }
}