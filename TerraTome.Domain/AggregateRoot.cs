namespace TerraTome.Domain
{
    public abstract record AggregateRoot<TAgg, TDto> : Entity
        where TAgg : AggregateRoot<TAgg, TDto>
        where TDto : class
    {
        protected AggregateRoot() : this(Guid.NewGuid())
        {
        }

        protected AggregateRoot(Guid id) : base(id)
        {
        }

        public abstract TDto ToDto();
    }
}