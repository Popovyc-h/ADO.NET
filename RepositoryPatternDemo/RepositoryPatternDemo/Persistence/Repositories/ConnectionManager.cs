using System.Data.Common;

namespace RepositoryPatternDemo.Persistence.Repositories;

internal class ConnectionManager
{
    //DbProviderFactories.RegisterFactory("System.Data.SQLite", SqliteFactory.Instance);
    public string ConnectionString { get; init; }
    public string ProviderName { get; init; }
    public DbProviderFactory DbProviderFactory { get; init; }

    public ConnectionManager(string connectionString, string providerName, DbProviderFactory dbProviderFactory)
    {
        ConnectionString = connectionString;
        ProviderName = providerName;
        DbProviderFactory = dbProviderFactory;
    }

    public DbConnection GetConnection()
    {
        var dbConnection = DbProviderFactory.CreateConnection()!;
        dbConnection.ConnectionString = ConnectionString;
        return dbConnection;
    }
}
