using Microsoft.Data.SqlClient;
using System.Data;

namespace level3;

internal class Program
{
    public static void AddTask(string title)
    {
        string connectionString =
            "Server=localhost,1433;Database=AcademyDB;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True;";

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand cmd = new SqlCommand("INSERT INTO Tasks (Title) OUTPUT INSERTED.Id VALUES (@Title)", connection);

            SqlParameter TitleParam = new SqlParameter
            {
                ParameterName = "@Title",
                SqlDbType = SqlDbType.NVarChar,
                Size = 200,
                Value = title
            };

            cmd.Parameters.Add(TitleParam);

            int idOfTheNewRecord = Convert.ToInt32(cmd.ExecuteScalar());

            Console.WriteLine($"Id of the new record: {idOfTheNewRecord}");
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void showTasks()
    {
        string connectionString = 
            "Server=localhost,1433;Database=AcademyDB;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True;";

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand cmd = new SqlCommand("SELECT Id, Title, IsCompleted, CreatedAt FROM Tasks WHERE IsCompleted = 0", connection);

            using SqlDataReader reader = cmd.ExecuteReader();

            int IdOrd = reader.GetOrdinal("Id");
            int TitleOrd = reader.GetOrdinal("Title");
            int IsCompletedOrd = reader.GetOrdinal("IsCompleted");
            int CreatedAtOrd = reader.GetOrdinal("CreatedAt");

            while (reader.Read())
            {
                int Id = reader.GetInt32(IdOrd);
                string Title = reader.GetString(TitleOrd);
                bool IsCompleted = reader.GetBoolean(IsCompletedOrd);
                DateTime CreatedAt = reader.GetDateTime(CreatedAtOrd);

                Console.WriteLine($"Id: {Id}\n" +
                    $"Title: {Title}\n" +
                    $"IsCompleted: {IsCompleted}\n" +
                    $"CreatedAt: {CreatedAt}\n");
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void DeleteAll()
    {
        string connectionString =
            "Server=localhost,1433;Database=AcademyDB;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True;";

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand cmd = new SqlCommand("TRUNCATE TABLE Tasks", connection);

            int rowsAffected = cmd.ExecuteNonQuery();

            Console.WriteLine($"Deleted {rowsAffected} rows");
            Console.WriteLine("All tasks deleted.");
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void CompleteTask(int Id)
    {
        string connectionString =
            "Server=localhost,1433;Database=AcademyDB;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True;";

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand cmd = new SqlCommand("UPDATE Tasks SET IsCompleted = 1 WHERE Id = @Id", connection);

            SqlParameter IdParam = new SqlParameter
            {
                ParameterName = "@Id",
                SqlDbType = SqlDbType.Int,
                Value = Id
            };

            cmd.Parameters.Add(IdParam);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
                Console.WriteLine($"Task {Id} marked as completed!");
            else
                Console.WriteLine($"Task with Id {Id} not found.");
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    static void Main(string[] args)
    {
        int choice;

        do
        {
            Console.WriteLine("\n1. Add task");
            Console.WriteLine("2. View the list");
            Console.WriteLine("3. Mark as completed");
            Console.WriteLine("4. Delete all");
            Console.WriteLine("0. Exit");
            
            Console.Write("Enter your choice: ");
            choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                Console.Write("Enter task title: ");
                string title = Console.ReadLine();

                AddTask(title);
            }
            else if (choice == 2)
            {
                showTasks();
            }
            else if (choice == 3)
            {
                Console.Write("Enter task ID: ");
                int Id = int.Parse(Console.ReadLine());

                CompleteTask(Id);
            }
            else if (choice == 4)
            {
                DeleteAll();
            }
            else if (choice == 0)
            {
                Console.WriteLine("Goodbye");
                break;
            }
        } while (choice != 0);
    }
}