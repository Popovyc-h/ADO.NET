using RepositoryPatternDemo.Persistence.Exceptions;

namespace RepositoryPatternDemo.Persistence.Entities;

internal class Post : IEntity, IComparable<Post>
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


    private string _slug;
    public string Slug
    {
        get => _slug;
        set
        {
            _slug = value;
            ValidateSlug();
        }
    }

    private string _title;
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            ValidateTitle();
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

    private string? _image;
    public string? Image
    {
        get => _image;
        set
        {
            _image = value;
            ValidateImage();
        }
    }

    public IEnumerable<Tag> Tags { get; set; }

    private DateTime? _publishedAt;
    public DateTime? PublishedAt
    {
        get => _publishedAt;
        set
        {
            _publishedAt = value;
            ValidatePublishedAt();
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

    private Post() { }

    public Post(Guid? id, User user, string slug, string title, string body, string? image, DateTime? publishedAt, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        User = user;
        Slug = slug;
        Title = title;
        Body = body;
        Image = image;
        PublishedAt = publishedAt;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;

        foreach (var error in Errors)
        {
            if (error.Value.Count > 0)
                throw new EntityValidationException(Errors);
        }
    }

    public int CompareTo(Post? other)
    {
        return PublishedAt?.CompareTo(other?.PublishedAt) ?? 0;
    }

    private void ValidateUser()
    {
        Errors[nameof(User)] = new List<string>();

        if (User is null)
        {
            Errors[nameof(User)].Add("Повинен бути автор поста.");
        }
    }

    private void ValidateSlug()
    {
        Errors[nameof(Slug)] = new List<string>();

        if (string.IsNullOrWhiteSpace(Slug))
        {
            Errors[nameof(Slug)].Add("Slug cannot be empty.");
        }
    }

    private void ValidateTitle()
    {
        Errors[nameof(Title)] = new List<string>();

        if (string.IsNullOrWhiteSpace(Title))
        {
            Errors[nameof(Title)].Add("Title cannot be empty.");
        }
    }

    private void ValidateBody()
    {
        Errors[nameof(Body)] = new List<string>();

        if (string.IsNullOrWhiteSpace(Body))
        {
            Errors[nameof(Body)].Add("Body cannot be empty.");
        }
    }

    private void ValidateImage()
    {
        Errors[nameof(Image)] = new List<string>();

        // No specific validation for Image, but you can add if needed
    }

    private void ValidatePublishedAt()
    {
        Errors[nameof(PublishedAt)] = new List<string>();

        if (PublishedAt.HasValue && PublishedAt.Value > DateTime.Now)
        {
            Errors[nameof(PublishedAt)].Add("PublishedAt cannot be in the future.");
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
        return $"Post: {Title}, User: {User.Name}, PublishedAt: {PublishedAt}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}, Id: {Id}";
    }
}
