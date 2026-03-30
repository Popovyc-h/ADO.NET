using RepositoryPatternDemo.Persistence.Entities;
using RepositoryPatternDemo.Persistence.Repositories.Contracts;
using RepositoryPatternDemo.Persistence.Repositories.Generics;
using System.Data.Common;

namespace RepositoryPatternDemo.Persistence.Repositories.AdoImpl;

internal class UserAdoRepository :
    GenericAdoRepository<User>,
    IUserRepository
{
    public UserAdoRepository(ConnectionManager connectionManager) :
        base(connectionManager, "Users")
    { }

    public User? GetByEmail(string email)
    {
        string query = $"SELECT * FROM {TableName} WHERE Email = @Email;";

        using var connection = ConnectionManager.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = query;
        var emailParameter = command.CreateParameter();
        emailParameter.ParameterName = "@Email";
        emailParameter.Value = email;
        command.Parameters.Add(emailParameter);

        using var reader = command.ExecuteReader();
        // Since Map is private in base class, I should probably make it protected or just use Get/GetAll if possible.
        // I'll change Map to protected in GenericAdoRepository.
        return reader.Read() ? Map(reader) : default;
    }

    public IEnumerable<User> GetByName(string name)
    {
        string query = $"SELECT * FROM {TableName} WHERE Name LIKE @Name;";

        using var connection = ConnectionManager.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = query;
        var nameParameter = command.CreateParameter();
        nameParameter.ParameterName = "@Name";
        nameParameter.Value = $"%{name}%";
        command.Parameters.Add(nameParameter);

        var items = new List<User>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
            items.Add(Map(reader));

        return items;
    }
}
