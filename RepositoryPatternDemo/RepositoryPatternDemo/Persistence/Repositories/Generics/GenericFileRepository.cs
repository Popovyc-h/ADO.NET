using RepositoryPatternDemo.Persistence.Entities;

namespace RepositoryPatternDemo.Persistence.Repositories.Generics;

internal abstract class GenericFileRepository<T> : IRepository<T> where T : IEntity
{
    private List<T> Entities { get; set; } = new List<T>();
    public readonly string filePath;

    protected GenericFileRepository(string filePath)
    {
        this.filePath = filePath;
        GetAll();
    }

    public T? Find(Predicate<T> predicate) => Entities.Find(predicate);

    public T? Get(Guid id) => Find((e) => e.Id.Equals(id));

    public IEnumerable<T> GetAll() => DeserializeAll();

    public void Add(T entity) => Entities.Add(entity);

    public void Remove(Guid id) => Entities.RemoveAll(e => e.Id == id);

    public void Remove(T entity) => Entities.Remove(entity);

    protected abstract T DeserializeEntity(string line);

    protected abstract string SerializeEntity(T entity);

    public void SerializeAll()
    {
        var _entities = new List<string>();

        foreach (var entity in Entities)
            _entities.Add(SerializeEntity(entity));

        File.WriteAllLines(filePath, _entities.ToArray());
    }

    private IEnumerable<T> DeserializeAll()
    {
        if (Entities.Count != 0) return Entities;
        if (!File.Exists(filePath)) return Enumerable.Empty<T>();

        foreach (var line in File.ReadAllLines(filePath))
        {
            Entities.Add(DeserializeEntity(line));
        }

        return Entities;
    }
}
