using System.Text.Json;
using Ardalis.Result;
using TerraTome.Domain.Dtos;

namespace TerraTome.Domain;

public class TerraTomeProject : AggregateRoot
{
    public string MonetaryUnit { get; private set; } = "Gold";
    public string Name { get; private set; } = "New World";
    public string Notes { get; private set; } = string.Empty;
    public string TimelineUnit { get; private set; } = "Year";

    public static Result<TerraTomeProject> TryCreate()
    {
        return new TerraTomeProject();
    }

    public override void FromDto(TerraTomeProjectDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        TimelineUnit = dto.TimelineUnit;
        MonetaryUnit = dto.MonetaryUnit;
        Notes = dto.Notes;
    }

    public void SetMonetaryUnit(string monetaryUnit)
    {
        MonetaryUnit = monetaryUnit;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetNotes(string notes)
    {
        Notes = notes;
    }

    public void SetTimelineUnit(string timelineUnit)
    {
        TimelineUnit = timelineUnit;
    }

    public override TerraTomeProjectDto ToDto(TerraTomeProjectDto? existingDto = null)
    {
        var dto = existingDto ?? new TerraTomeProjectDto();
        dto.Id = Id;
        dto.Name = Name;
        dto.TimelineUnit = TimelineUnit;
        dto.MonetaryUnit = MonetaryUnit;
        dto.Notes = Notes;

        return dto;
    }
}
