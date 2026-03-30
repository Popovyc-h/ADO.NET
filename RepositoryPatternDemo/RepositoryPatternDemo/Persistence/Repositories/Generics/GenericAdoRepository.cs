using RepositoryPatternDemo.Persistence.Entities;
using System.Data.Common;
using System.Reflection;

namespace RepositoryPatternDemo.Persistence.Repositories.Generics;

internal abstract class GenericAdoRepository<T> : IRepository<T> where T : IEntity
{
    public ConnectionManager ConnectionManager { get; init; }
    public string TableName { get; init; }

    protected GenericAdoRepository(ConnectionManager connectionManager, string? tableName = null)
    {
        ConnectionManager = connectionManager;
        TableName = tableName ?? $"{typeof(T).Name}s";
    }

    public T? Get(Guid id)
    {
        string query = $"SELECT * FROM {TableName} WHERE Id = @Id;";

        using var connection = ConnectionManager.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = query;
        var idParameter = command.CreateParameter();
        idParameter.ParameterName = "@Id";
        idParameter.Value = id;
        command.Parameters.Add(idParameter);

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : default;
    }

    public IEnumerable<T> GetAll()
    {
        string query = $"SELECT * FROM {TableName};";

        using var connection = ConnectionManager.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = query;

        var items = new List<T>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
            items.Add(Map(reader));

        return items;
    }

    public T? Find(Predicate<T> predicate)
    {
        return GetAll().FirstOrDefault(t => predicate(t));
    }

    public void Remove(Guid id)
    {
        string query = $"DELETE FROM {TableName} WHERE Id = @Id;";

        using var connection = ConnectionManager.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = query;
        var idParameter = command.CreateParameter();
        idParameter.ParameterName = "@Id";
        idParameter.Value = id;
        command.Parameters.Add(idParameter);

        command.ExecuteNonQuery();
    }

    public void Remove(T entity)
    {
        if (entity.Id.HasValue)
            Remove(entity.Id.Value);
    }

    public void Add(T entity)
    {
        using var connection = ConnectionManager.GetConnection();
        connection.Open();

        if (entity.Id == null)
            Insert(entity, connection);
        else
            Update(entity, connection);
    }

    private void Update(T entity, DbConnection connection)
    {
        var properties = GetMappableProperties().ToList();
        var updateCommand = connection.CreateCommand();
        string setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

        updateCommand.CommandText = $"UPDATE {TableName} SET {setClause} WHERE Id = @Id;";

        InitCommandParameters(updateCommand, entity, properties);

        var idParameter = updateCommand.CreateParameter();
        idParameter.ParameterName = "@Id";
        idParameter.Value = entity.Id;
        updateCommand.Parameters.Add(idParameter);

        updateCommand.ExecuteNonQuery();
    }

    private void Insert(T entity, DbConnection connection)
    {
        var properties = GetMappableProperties().ToList();
        var insertCommand = connection.CreateCommand();
        string attributes = string.Join(", ", properties.Select(p => p.Name));
        string parameters = string.Join(", ", properties.Select(p => $"@{p.Name}"));

        Guid id = Guid.NewGuid();
        insertCommand.CommandText = $"INSERT INTO {TableName} (Id, {attributes}) VALUES (@Id, {parameters});";

        var idParameter = insertCommand.CreateParameter();
        idParameter.ParameterName = "@Id";
        idParameter.Value = id;
        insertCommand.Parameters.Add(idParameter);

        InitCommandParameters(insertCommand, entity, properties);

        insertCommand.ExecuteNonQuery();
        entity.Id = id;
    }

    private void InitCommandParameters(DbCommand command, T entity, IEnumerable<PropertyInfo> properties)
    {
        foreach (var prop in properties)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = $"@{prop.Name}";
            parameter.Value = prop.GetValue(entity) ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }
    }

    protected T Map(DbDataReader reader)
    {
        T instance = (T)Activator.CreateInstance(typeof(T), true)!;
        
        var idOrdinal = reader.GetOrdinal("Id");
        if (idOrdinal >= 0 && !reader.IsDBNull(idOrdinal))
        {
            instance.Id = reader.GetGuid(idOrdinal);
        }

        foreach (var prop in GetMappableProperties())
        {
            int ordinal = -1;
            try { ordinal = reader.GetOrdinal(prop.Name); } catch { /* skip if column not found */ }
            
            if (ordinal >= 0)
            {
                object value = reader.GetValue(ordinal);
                if (value == DBNull.Value)
                {
                    prop.SetValue(instance, null);
                }
                else
                {
                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    prop.SetValue(instance, Convert.ChangeType(value, targetType));
                }
            }
        }
        return instance;
    }

    private IEnumerable<PropertyInfo> GetMappableProperties()
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.Name != "Id" && p.CanWrite && p.CanRead && IsSimpleType(p.PropertyType));
    }

    private bool IsSimpleType(Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
        return underlyingType.IsPrimitive || 
               underlyingType == typeof(string) || 
               underlyingType == typeof(decimal) || 
               underlyingType == typeof(DateTime) || 
               underlyingType == typeof(Guid) ||
               underlyingType.IsEnum;
    }
}
