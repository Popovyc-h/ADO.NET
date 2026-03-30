using RepositoryPatternDemo.Persistence.Entities;
using RepositoryPatternDemo.Persistence.Repositories.Contracts;
using RepositoryPatternDemo.Persistence.Repositories.Generics;

namespace RepositoryPatternDemo.Persistence.Repositories.Impl;

internal class UserFileRepository : GenericFileRepository<User>, IUserRepository
{
    public UserFileRepository() : base(Path.Combine("Data", "users.txt")) { }

    public User? GetByEmail(string email)
    {
        return Find((e) => e.Email == email);
    }

    public IEnumerable<User> GetByName(string name)
    {
        return GetAll().Where(x => x.Name == name).ToList();
    }

    protected override User DeserializeEntity(string line)
    {
        string[] userFields = line.Split("|");
        return new User(
                id: Guid.Parse(userFields[0]),
                name: userFields[1],
                email: userFields[2],
                password: userFields[3],
                avatar: !string.IsNullOrEmpty(userFields[4]) ? userFields[4] : null,
                createdAt: DateTime.Parse(userFields[5]),
                updatedAt: DateTime.Parse(userFields[6])
            );
    }

    protected override string SerializeEntity(User user)
    {
        return $"{user.Id}|{user.Name}|{user.Email}|{user.Password}|{user.Avatar}|{user.CreatedAt}|{user.UpdatedAt}";
    }
}
