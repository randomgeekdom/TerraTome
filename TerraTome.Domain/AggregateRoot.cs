namespace TerraTome.Domain
{
    public record AggregateRoot : Entity
    {
        protected AggregateRoot() : this(Guid.NewGuid())
        {
        }

        protected AggregateRoot(Guid id) : base(id)
        {
        }
    }
}