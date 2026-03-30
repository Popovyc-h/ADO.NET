using RepositoryPatternDemo.Persistence.Exceptions;

namespace RepositoryPatternDemo.Persistence.Entities;

internal class User : IEntity, IComparable<User>
{

    public Guid? Id { get; set; }

    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            ValidateName();
        }
    }

    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            ValidateEmail();
        }
    }

    private string _password;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            ValidatePassword();
        }
    }

    private string? _avatar;
    public string? Avatar
    {
        get => _avatar;
        set
        {
            _avatar = value;
            ValidateAvatar();
        }
    }

    private DateTime _createdAt;
    public DateTime CreatedAt
    {
        get => _createdAt;
        set
        {
            _createdAt = value;
            ValidateCreatedAt();
        }
    }

    private DateTime _updatedAt;
    public DateTime UpdatedAt
    {
        get => _updatedAt;
        set
        {
            _updatedAt = value;
            ValidateUpdatedAt();
        }
    }

    private Dictionary<string, List<string>> Errors { get; set; } = new();

    private User() { }

    public User(
        Guid id,
       string name,
       string email,
       string password,
       string? avatar,
       DateTime createdAt,
       DateTime updatedAt) : this(name, email, password, avatar, createdAt, updatedAt)
    {
        Id = id;
    }

    public User(
    string name,
    string email,
    string password,
    string? avatar,
    DateTime createdAt,
    DateTime updatedAt)
    {
        Name = name;
        Email = email;
        Password = password;
        Avatar = avatar;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;

        foreach (var error in Errors)
        {
            if (error.Value.Count > 0)
                throw new EntityValidationException(Errors);
        }
    }

    public int CompareTo(User? other)
    {
        return Name.CompareTo(other?.Name);
    }

    private void ValidateName()
    {
        Errors[nameof(Name)] = new List<string>();

        if (string.IsNullOrWhiteSpace(Name))
        {
            Errors[nameof(Name)].Add("Name cannot be empty.");
        }
    }

    private void ValidateEmail()
    {
        Errors[nameof(Email)] = new List<string>();

        if (string.IsNullOrWhiteSpace(Email))
        {
            Errors[nameof(Email)].Add("Email cannot be empty.");
        }
        else if (!Email.Contains("@"))
        {
            Errors[nameof(Email)].Add("Email is not valid.");
        }
    }

    private void ValidatePassword()
    {
        Errors[nameof(Password)] = new List<string>();

        if (string.IsNullOrWhiteSpace(Password))
        {
            Errors[nameof(Password)].Add("Password cannot be empty.");
        }
        else if (Password.Length < 6)
        {
            Errors[nameof(Password)].Add("Password must be at least 6 characters long.");
        }
    }

    private void ValidateAvatar()
    {
        Errors[nameof(Avatar)] = new List<string>();

        // No specific validation for Avatar, but you can add if needed
    }

    private void ValidateCreatedAt()
    {
        Errors[nameof(CreatedAt)] = new List<string>();

        if (CreatedAt > DateTime.Now)
        {
            Errors[nameof(CreatedAt)].Add("CreatedAt cannot be in the future.");
        }
    }

    private void ValidateUpdatedAt()
    {
        Errors[nameof(UpdatedAt)] = new List<string>();

        if (UpdatedAt > DateTime.Now)
        {
            Errors[nameof(UpdatedAt)].Add("UpdatedAt cannot be in the future.");
        }

        if (UpdatedAt < CreatedAt)
        {
            Errors[nameof(UpdatedAt)].Add("UpdatedAt cannot be before CreatedAt.");
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is IEntity entity)
        {
            return Id.Equals(entity.Id);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string? ToString()
    {
        return $"User: {Name}, Email: {Email}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
}
