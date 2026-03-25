namespace ConsoleApp1.Models;

public class Author
{
    private int id;
    private string name;
    private string surname;

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

    public string Name
    {
        get => name;

        set
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            name = value;
        }
    }

    public string Surname
    {
        get => surname;

        set
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            surname = value;
        }
    }
}