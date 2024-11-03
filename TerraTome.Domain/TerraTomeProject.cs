using System.Text.Json;
using Ardalis.Result;
using TerraTome.Domain.Dtos;

namespace TerraTome.Domain;

public class TerraTomeProject : Entity<TerraTomeProjectDto>
{
	private TerraTomeProject(string filePath)
	{
		_filePath = filePath;
	}

	public static Result<TerraTomeProject> TryCreate(string filePath)
	{
		return new TerraTomeProject(filePath);
	}

	private readonly string _filePath;

	public override TerraTomeProjectDto ToDto()
	{
		return new TerraTomeProjectDto
		{
			Id = Id,
		};
	}

	public override void FromDto(TerraTomeProjectDto dto)
	{
		Id = dto.Id;
	}

	public async Task<Result> SaveAsync()
	{
		await using var stream = new FileStream(_filePath, FileMode.Create);
		await JsonSerializer.SerializeAsync(stream, ToDto());
		return Result.Success();
	}
}