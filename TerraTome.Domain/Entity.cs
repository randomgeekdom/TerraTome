namespace TerraTome.Domain
{
    public abstract record Entity
    {
        protected Entity() : this(Guid.NewGuid())
        {
        }

        protected Entity(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; } = Guid.NewGuid();
    }
}