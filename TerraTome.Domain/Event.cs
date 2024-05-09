using CSharpFunctionalExtensions;

namespace TerraTome.Domain
{
    public record Event : AggregateRoot
    {
        public string Name { get; }
        public long Cycle { get; }

        /// <summary>
        /// Constructor used for serialization and deserialization
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="cycle"></param>
        public Event(Guid id, string name, long cycle) : base(id)
        {
            this.Name = name;
            this.Cycle = cycle;
        }

        public static Result<Event> TryCreate(string name, long cycle)
        {
            return Result.Success(new Event(Guid.NewGuid(), name, cycle));
        }
    }
}