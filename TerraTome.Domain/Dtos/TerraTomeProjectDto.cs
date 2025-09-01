namespace TerraTome.Domain.Dtos;

public class TerraTomeProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
	public string TimelineUnit { get; set; } = "Years";
	public string MonetaryUnit { get; set; } = "Gold";
	public string Notes { get; set; } = string.Empty;
	public AttributeDto[] Attributes { get; set; } = [];
	public IEnumerable<string> OpenTabs { get; set; } = [];
}