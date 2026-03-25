using ConsoleApp1.Database;
using ConsoleApp1.Models;
using System.Data.SqlClient;

namespace ConsoleApp1.Repositories;

public class AuthorRepository
{
    private DatabaseHelper Db;

    public AuthorRepository(DatabaseHelper Db)
    {
        this.Db = Db;
    }

    public void GetAllAuthors()
    {
        using var conn = Db.GetConnection();
        conn.Open();

        using SqlCommand cmd = new SqlCommand("SELECT Id, Name, Surname FROM Authors", conn);

        using SqlDataReader reader = cmd.ExecuteReader();

        int IdOrd = reader.GetOrdinal("Id");
        int NameOrd = reader.GetOrdinal("Name");
        int SurnameOrd = reader.GetOrdinal("Surname");

        while (reader.Read())
        {
            int Id = reader.GetInt32(IdOrd);
            string Name = reader.GetString(NameOrd);
            string Surname = reader.GetString(SurnameOrd);

            Console.WriteLine($"Id: {Id}\n" +
                $"Name: {Name}\n" +
                $"Surname: {Surname}\n");
        }
    }

    public void AddAuthor(Author author)
    {
        using var conn = Db.GetConnection();
        conn.Open();

        using SqlCommand cmd = new SqlCommand("INSERT INTO Authors VALUES (@Name, @Surname) ", conn);

        SqlParameter NameParam = new SqlParameter
        {
            ParameterName = "@Name",
            SqlDbType = System.Data.SqlDbType.NVarChar,
            Size = 150,
            Value = author.Name
        };

        SqlParameter SurnameParam = new SqlParameter
        {
            ParameterName = "@Surname",
            SqlDbType = System.Data.SqlDbType.NVarChar,
            Size = 100,
            Value = author.Surname
        };

        cmd.Parameters.Add(NameParam);
        cmd.Parameters.Add(SurnameParam);

        cmd.ExecuteNonQuery();
        Console.WriteLine("Add new author");
    }

    public void DeleteAuthor(int id)
    {
        using var conn = Db.GetConnection();
        conn.Open();

        using SqlCommand cmd = new SqlCommand("DELETE FROM Authors WHERE Id = @Id", conn);
        using SqlCommand countCmd = new SqlCommand("SELECT COUNT(Id) FROM Books WHERE AuthorId = @AuthorId", conn);

        SqlParameter IdParam = new SqlParameter
        {
            ParameterName = "@Id",
            SqlDbType = System.Data.SqlDbType.Int,
            Value = id
        };

        SqlParameter AuthorIdParam = new SqlParameter
        {
            ParameterName = "@AuthorId",
            SqlDbType = System.Data.SqlDbType.Int,
            Value = id
        };

        countCmd.Parameters.Add(AuthorIdParam);

        object? result = countCmd.ExecuteScalar();
        int count = Convert.ToInt32(result);

        if (count > 0)
        {
            Console.WriteLine($"You cannot delete an author because he has {count} books.");
            return;
        }

        cmd.Parameters.Add(IdParam);
        cmd.ExecuteNonQuery();
        Console.WriteLine($"Author removed from ID: {id}");
    }
}