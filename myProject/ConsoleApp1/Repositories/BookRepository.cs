using ConsoleApp1.Database;
using ConsoleApp1.Models;
using System.Data.SqlClient;

namespace ConsoleApp1.Repositories;

public class BookRepository
{
    private DatabaseHelper Db;

    public BookRepository(DatabaseHelper Db)
    {
        this.Db = Db;
    }

    public void GetAllBooks()
    {
        using var conn = Db.GetConnection();
        conn.Open();

        using SqlCommand cmd = new SqlCommand("SELECT Id, Title, PublishedYear, AuthorId FROM Books", conn);

        using SqlDataReader reader = cmd.ExecuteReader();

        int IdOrd = reader.GetOrdinal("Id");
        int TitleOrd = reader.GetOrdinal("Title");
        int PublishedYearOrd = reader.GetOrdinal("PublishedYear");
        int AuthorIdOrd = reader.GetOrdinal("AuthorId");

        while (reader.Read())
        {
            int Id = reader.GetInt32(IdOrd);
            string Title = reader.GetString(TitleOrd);
            int PublishedYear = reader.GetInt32(PublishedYearOrd);
            int AuthorId = reader.GetInt32(AuthorIdOrd);

            Console.WriteLine($"Id: {Id}\n" +
                $"Title: {Title}\n" +
                $"PublishedYear: {PublishedYear}\n" +
                $"AuthorId: {AuthorId}\n");
        }
    }

    public void AddBook(Book book)
    {
        using var conn = Db.GetConnection();
        conn.Open();

        using SqlCommand cmd = new SqlCommand("INSERT INTO Books VALUES (@Title, @PublishedYear, @AuthorId)", conn);

        SqlParameter titleParam = new SqlParameter
        {
            ParameterName = "@Title",
            SqlDbType = System.Data.SqlDbType.NVarChar,
            Size = 100,
            Value = book.Title
        };

        SqlParameter PublishedYearParam = new SqlParameter
        {
            ParameterName = "@PublishedYear",
            SqlDbType = System.Data.SqlDbType.Int,
            Value = book.PublishedYear
        };

        SqlParameter AuthorIdParam = new SqlParameter
        {
            ParameterName = "@AuthorId",
            SqlDbType = System.Data.SqlDbType.Int,
            Value = book.AuthorId
        };

        cmd.Parameters.Add(titleParam);
        cmd.Parameters.Add(PublishedYearParam);
        cmd.Parameters.Add(AuthorIdParam);

        cmd.ExecuteNonQuery();
        Console.WriteLine("Add new books");
    }

    public void DeleteBook(int id)
    {
        using var conn = Db.GetConnection();
        conn.Open();

        using SqlCommand cmd = new SqlCommand("DELETE FROM Books WHERE Id = @Id", conn);

        SqlParameter IdParam = new SqlParameter
        {
            ParameterName = "@Id",
            SqlDbType = System.Data.SqlDbType.Int,
            Value = id
        };

        cmd.Parameters.Add(IdParam);
        cmd.ExecuteNonQuery();
        Console.WriteLine($"Deleted book from ID: {id}");
    }
}