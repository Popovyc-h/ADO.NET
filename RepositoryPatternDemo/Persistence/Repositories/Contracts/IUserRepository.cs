using RepositoryPatternDemo.Persistence.Entities;

namespace RepositoryPatternDemo.Persistence.Repositories.Contracts;

internal interface IUserRepository : IRepository<User>
{
    IEnumerable<User> GetByName(string name);
    User? GetByEmail(string email);
}
