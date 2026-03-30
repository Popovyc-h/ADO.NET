using RepositoryPatternDemo.Persistence.Entities;

namespace RepositoryPatternDemo.Persistence.Repositories;

internal interface IRepository<T> where T : IEntity
{
    T? Get(Guid id);
    IEnumerable<T> GetAll();
    T? Find(Predicate<T> predicate);
    void Remove(Guid id);
    void Remove(T entity);
    void Add(T entity);
}
