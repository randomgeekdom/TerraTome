using System.Text.Json;
using Ardalis.Result;
using TerraTome.Domain.Dtos;

namespace TerraTome.Domain;

public class TerraTomeProject : Entity<TerraTomeProjectDto>
{
    private Uri _filePath;

    private TerraTomeProject(Uri filePath)
    {
        _filePath = filePath;
    }

    public string MonetaryUnit { get; private set; } = "Gold";
    public string Name { get; private set; } = "New World";
    public string Notes { get; private set; } = string.Empty;
    public string TimelineUnit { get; private set; } = "Year";

    public static Result<TerraTomeProject> TryCreate(Uri filePath)
    {
        return new TerraTomeProject(filePath);
    }

    public override void FromDto(TerraTomeProjectDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        TimelineUnit = dto.TimelineUnit;
        MonetaryUnit = dto.MonetaryUnit;
        Notes = dto.Notes;
    }

    public Uri GetFilePath()
    {
        return _filePath;
    }

    public void SetFilePath(Uri filePath)
    {
        this._filePath = filePath;
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

    public override TerraTomeProjectDto ToDto()
    {
        return new TerraTomeProjectDto
        {
            Id = Id,
            Name = Name,
            TimelineUnit = TimelineUnit,
            MonetaryUnit = MonetaryUnit,
            Notes = Notes
        };
    }
}