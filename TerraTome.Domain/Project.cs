using CSharpFunctionalExtensions;
using TerraTome.Domain.Dtos;

namespace TerraTome.Domain
{
    public record Project
    {
        public string Path { get; set; }
        public List<EventDto> Events { get; } = new List<EventDto>();
        public string Name { get; set; }
    }
}