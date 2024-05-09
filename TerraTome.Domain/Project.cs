namespace TerraTome.Domain
{
    public record Project : AggregateRoot
    {
        public Project()
        {
            this.Name = string.Empty;
            this.Events = new List<Event>();
        }

        public Project(string name, List<Event> events)
        {
            this.Name = name;
            this.Events = events;
        }
        public string Name { get; set; }

        public List<Event> Events { get; }
    }
}