using Ardalis.Result;
using System.Linq.Expressions;
using TerraTome.Domain.Dtos;

namespace TerraTome.Domain;

public class TerraTomeProjectSettings: Entity<TerraTomeProjectDto>
{
    private Uri _filePath;

    private TerraTomeProjectSettings(Uri filePath)
    {
        _filePath = filePath;
    }

    public static Result<TerraTomeProjectSettings> TryCreate(Uri filePath)
    {
        return new TerraTomeProjectSettings(filePath);
    }

    public override void FromDto(TerraTomeProjectDto dto)
    {
        Id = dto.Id;
    }

    public Uri GetFilePath()
    {
        return _filePath;
    }

    public void SetFilePath(Uri file)
    {
        _filePath = file;
    }

    public override TerraTomeProjectDto ToDto(TerraTomeProjectDto? existingDto = null)
    {
        var dto = existingDto ?? new TerraTomeProjectDto();
        dto.Id = this.Id;
        return dto;
    }
}