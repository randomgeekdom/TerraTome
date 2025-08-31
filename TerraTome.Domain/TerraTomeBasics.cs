using System.Text.Json;
using Ardalis.Result;
using TerraTome.Domain.Dtos;

namespace TerraTome.Domain;

public class TerraTomeBasics : AggregateRoot
{
    public string MonetaryUnit { get; private set; } = "Gold";
    public string Name { get; private set; } = "New World";
    public string Notes { get; private set; } = string.Empty;
    public Dictionary<string, double> NumericAttributes { get; private set; } = [];
    public Dictionary<string, string> TextAttributes { get; private set; } = [];
    public string TimelineUnit { get; private set; } = "Year";

    public static Result<TerraTomeBasics> TryCreate()
    {
        return new TerraTomeBasics();
    }

    public override void FromDto(TerraTomeProjectDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        TimelineUnit = dto.TimelineUnit;
        MonetaryUnit = dto.MonetaryUnit;
        Notes = dto.Notes;

        foreach (var attribute in dto.Attributes)
        {
            if (attribute.Type == typeof(double) && attribute.Value is double dValue)
            {
                NumericAttributes[attribute.Name] = dValue;
            }
            else if (attribute.Type == typeof(string) && attribute.Value is string sValue)
            {
                TextAttributes[attribute.Name] = sValue;
            }
        }
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

        dto.Attributes = [.. NumericAttributes.Select(kv => new AttributeDto
        {
            Name = kv.Key,
            Type = typeof(double),
            Value = kv.Value
        })];
        dto.Attributes =
        [
            .. dto.Attributes,
            .. TextAttributes.Select(kv => new AttributeDto
            {
                Name = kv.Key,
                Type = typeof(string),
                Value = kv.Value
            }),
        ];

        return dto;
    }

    public Result TryAddNumericAttribute(string name, double value)
    {
        return TryAddAttribute(name, value, NumericAttributes);
    }

    public Result TryAddTextAttribute(string name, string value)
    {
        return TryAddAttribute(name, value, TextAttributes);
    }

    private static Result TryAddAttribute<T>(string name, T value, Dictionary<string, T> dictionary)
    {
        if (dictionary.ContainsKey(name))
        {
            return Result.Error("An attribute with the name '{name}' already exists.");
        }
        dictionary[name] = value;
        return Result.Success();
    }
}