namespace level1;
using Microsoft.Data.SqlClient;
using System.Data;

internal class level1
{
    static void Main(string[] args)
    {
        string connectionString =
           "Server=localhost,1433;Database=AcademyDB;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True;";

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand countCmd = new SqlCommand("SELECT COUNT(Id) FROM Users", connection);
            var countResult = countCmd.ExecuteScalar();
            var result = Convert.ToInt32(countResult);
            Console.WriteLine($"Count: {result}\n");

            string userInput = "Ноутбук";
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE Name = @Name", connection);

            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.NVarChar,
                Size = 100,
                Value = userInput
            };

            cmd.Parameters.Add(nameParam);

            using SqlDataReader readerLevel1 = cmd.ExecuteReader();

            while (readerLevel1.Read())
            {
                int Id = readerLevel1.GetInt32(0);
                string Name = readerLevel1.GetString(1);
                decimal Price = readerLevel1.GetDecimal(2);
                bool IsAvailable = readerLevel1.GetBoolean(3);
                string? Description = readerLevel1.IsDBNull(4) ? null : readerLevel1.GetString(4);

                Console.WriteLine($"Id: {Id}" +
                    $"\nName: {Name}" +
                    $"\nPrice: {Price}" +
                    $"\nIsAvailable: {IsAvailable}" +
                    $"\nDescription: {Description}\n");
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}