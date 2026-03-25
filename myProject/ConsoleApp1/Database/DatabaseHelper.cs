using System.Data.SqlClient;

namespace ConsoleApp1.Database;

public class DatabaseHelper
{
   private string connectionString =
          "Server=localhost,1433;Database=MyProject;User Id=sa;Password=Password123$;TrustServerCertificate=True;";

    public SqlConnection GetConnection()
    {
        SqlConnection conn = new SqlConnection(connectionString);

        return conn;
    }
}