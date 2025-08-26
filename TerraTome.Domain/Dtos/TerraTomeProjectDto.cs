namespace TerraTome.Domain.Dtos;

public class TerraTomeProjectDto:EntityDto
{
	public string Name { get; set; } = string.Empty;
	public string TimelineUnit { get; set; } = "Years";
	public string MonetaryUnit { get; set; } = "Gold";
	public string Notes { get; set; } = string.Empty;
}