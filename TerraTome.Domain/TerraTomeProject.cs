using System.Text.Json;
using Ardalis.Result;
using TerraTome.Domain.Dtos;

namespace TerraTome.Domain;

public class TerraTomeProject : Entity<TerraTomeProjectDto>
{
    private string _filePath;

    private TerraTomeProject(string filePath)
    {
        _filePath = filePath;
    }

    public string MonetaryUnit { get; private set; } = "Gold";
    public string Name { get; private set; } = "New World";
    public string Notes { get; private set; } = string.Empty;
    public string TimelineUnit { get; private set; } = "Year";

    public static Result<TerraTomeProject> TryCreate(string filePath)
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

    public string GetFilePath()
    {
        return _filePath;
    }

    public async Task<Result> SaveAsync()
    {
        await using var stream = new FileStream(_filePath, FileMode.Create);
        await JsonSerializer.SerializeAsync(stream, ToDto());
        return Result.Success();
    }

    public void SetFilePath(string filePath)
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