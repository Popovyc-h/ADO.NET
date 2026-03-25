using System.Data.SqlClient;

namespace level1;

internal class Program
{
    public bool Login(SqlConnection conn, string username, string password)
    {
        // ВРАЗЛИВИЙ КОД! Виправте його:
        //string sql = $"SELECT COUNT(*) FROM Users WHERE Username = '{username}' AND Password = '{password}'";
        //using SqlCommand cmd = new SqlCommand(sql, conn);


        // ВИПРАВЛЕНИЙ КОД
        using SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password", conn);

        SqlParameter nameParam = new SqlParameter
        {
            ParameterName = "@username",
            SqlDbType = System.Data.SqlDbType.NVarChar,
            Size = 50,
            Value = username
        };

        SqlParameter passwordParam = new SqlParameter
        {
            ParameterName = "@password",
            SqlDbType = System.Data.SqlDbType.NVarChar,
            Size = 255,
            Value = password
        };

        cmd.Parameters.Add(nameParam);
        cmd.Parameters.Add(passwordParam);

        int count = (int)cmd.ExecuteScalar();

        return count > 0;
    }

    public static void PrintProducts(SqlDataReader reader)
    {
        int descOrdinal = reader.GetOrdinal("Description");
        while (reader.Read())
        {
            // ДОДАЙТЕ ПЕРЕВІРКУ ТУТ:
            string? description = reader.IsDBNull(descOrdinal) ? "The description is empty" : reader.GetString(descOrdinal);

            Console.WriteLine($"Опис: {description}");
        }
    }

    static void Main(string[] args)
    {
        string connectionString =
          "Server=localhost,1433;Database=AdoNetAdvancedDB;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True;";

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            Program p = new Program();
            connection.Open();

            bool result = p.Login(connection, "' OR '1'='1' --", "wrongpassword");
            Console.WriteLine(result);

            using SqlCommand cmd = new SqlCommand("SELECT Name, Price, Description, Stock FROM Products", connection);

            using SqlDataReader reader = cmd.ExecuteReader();

            PrintProducts(reader);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}