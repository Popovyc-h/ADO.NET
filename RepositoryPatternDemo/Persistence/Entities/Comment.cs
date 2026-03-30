
using RepositoryPatternDemo.Persistence.Exceptions;

namespace RepositoryPatternDemo.Persistence.Entities;

internal class Comment : IEntity, IComparable<Comment>
{
    public Guid? Id { get; set; }

    private User _user;
    public User User
    {
        get => _user;
        set
        {
            _user = value;
            ValidateUser();
        }
    }

    private Post _post;
    public Post Post
    {
        get => _post;
        set
        {
            _post = value;
            ValidatePost();
        }
    }

    private string _body;
    public string Body
    {
        get => _body;
        set
        {
            _body = value;
            ValidateBody();
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

    private Comment() { }

    public Comment(Guid? id, User user, Post post, string body, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        User = user;
        Post = post;
        Body = body;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;

        foreach (var error in Errors)
        {
            if (error.Value.Count > 0)
                throw new EntityValidationException(Errors);
        }
    }

    public int CompareTo(Comment? other)
    {
        return CreatedAt.CompareTo(other?.CreatedAt);
    }

    private void ValidateUser()
    {
        Errors[nameof(User)] = new List<string>();

        if (User == null)
        {
            Errors[nameof(User)].Add("User cannot be null.");
        }
    }

    private void ValidatePost()
    {
        Errors[nameof(Post)] = new List<string>();

        if (Post == null)
        {
            Errors[nameof(Post)].Add("Post cannot be null.");
        }
    }

    private void ValidateBody()
    {
        Errors[nameof(Body)] = new List<string>();

        if (string.IsNullOrWhiteSpace(Body))
        {
            Errors[nameof(Body)].Add("Body cannot be empty.");
        }
        if (Body.Length > 256)
        {
            Errors[nameof(Body)].Add("Коментар заннадто великий.");
        }
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
        return $"Comment: {Body}, User: {User.Name}, Post: {Post.Title}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}, Id: {Id}";
    }
}