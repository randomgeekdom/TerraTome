using CSharpFunctionalExtensions;
using TerraTome.Domain.Dtos;

namespace TerraTome.Domain
{
    public record Event : AggregateRoot<Event, EventDto>
    {
        public string Name { get; }
        public long Period { get; }

        /// <summary>
        /// Constructor used for serialization and deserialization
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="period"></param>
        private Event(Guid id, string name, long period) : base(id)
        {
            this.Name = name;
            this.Period = period;
        }

        public static Result<Event> TryCreate(string name, long period)
        {
            return Result.Success(new Event(Guid.NewGuid(), name, period));
        }

        public static Result<Event> FromDto(EventDto dto)
        {
            return Result.Success(new Event(dto.Id, dto.Name, dto.Period));
        }

        public override EventDto ToDto()
        {
            return new EventDto(this.Id, this.Name, this.Period);
        }
    }
}