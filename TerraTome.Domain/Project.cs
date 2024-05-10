using CSharpFunctionalExtensions;

namespace TerraTome.Domain
{
    public record Project : AggregateRoot
    {
        private Project() : this(Guid.NewGuid(), "New Project", true, new List<Event>())
        {
        }

        public Project(Guid id, string name, bool isDirty, List<Event> events)
        {
            this.Id = id;
            this.Name = name;
            this.IsDirty = isDirty;
            this.Events = events;
        }

        public Guid Id { get; }
        public string Name { get; }
        public List<Event> Events { get; }

        public bool IsDirty { get; }

        public static Result<Project> TryCreate()
        {
            return new Project();
        }
    }
}