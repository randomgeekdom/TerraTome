using System.Text.Json;
using TerraTome.Domain;

namespace TerraTome.Infrastructure
{
    public class FileSaver : IFileSaver
    {
        public FileSaver(string baseFilePath)
        {
            this.BaseFilePath = baseFilePath;
        }

        public string BaseFilePath { get; }

        public async Task<T?> LoadAsync<T>(Guid id) where T : AggregateRoot
        {
            var json = await File.ReadAllTextAsync(Path.Combine(BaseFilePath, $"{id}.json"));
            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task<T> SaveAsync<T>(T entity) where T : AggregateRoot
        {
            var json = JsonSerializer.Serialize(entity);
            await File.WriteAllTextAsync(Path.Combine(BaseFilePath, $"{entity.Id}.json"), json);
            return entity;
        }
    }
}