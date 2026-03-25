namespace ConsoleApp1.Models;

public class Book
{
    private int id;
    private string title;
    private int publishedYear;
    private int authorId;

    public int Id
    {
        get => id;

        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            id = value;
        }
    }

    public string Title
    {
        get => title;

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            title = value;
        }
    }

    public int PublishedYear
    {
        get => publishedYear;

        set
        {
            if (value <= 1400 || value > DateTime.Now.Year)
                throw new ArgumentOutOfRangeException(nameof(value));

            publishedYear = value;
        }
    }

    public int AuthorId
    {
        get => authorId;

        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            authorId = value;
        }
    }
}