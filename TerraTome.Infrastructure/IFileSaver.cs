using TerraTome.Domain;

namespace TerraTome.Infrastructure
{
    public interface IFileSaver
    {
        Task<T?> LoadAsync<T>(Guid id) where T : AggregateRoot;

        Task<T> SaveAsync<T>(T entity) where T : AggregateRoot;
    }
}