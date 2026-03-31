using RepositoryPatternDemo.Persistence.Entities;
using RepositoryPatternDemo.Persistence.Repositories.Contracts;
using RepositoryPatternDemo.Persistence.Repositories.Generics;

namespace RepositoryPatternDemo.Persistence.Repositories.AdoImpl;

internal class TagAdoRepository :
    GenericAdoRepository<Tag>,
    ITagRepository
{
    public TagAdoRepository(ConnectionManager connectionManager) :
        base(connectionManager, "Tags")
    { }

    public Tag? GetBySlug(string slug)
    {
        string query = $"SELECT * FROM {TableName} WHERE Slug = @Slug";

        using var connection = ConnectionManager.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = query;
        var slugParameter = command.CreateParameter();
        slugParameter.ParameterName = "@Slug";
        slugParameter.Value = slug;
        command.Parameters.Add(slugParameter);

        using var reader = command.ExecuteReader();

        return reader.Read() ? Map(reader) : default;
    }
}
