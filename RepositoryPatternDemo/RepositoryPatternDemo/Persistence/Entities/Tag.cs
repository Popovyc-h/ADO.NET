using RepositoryPatternDemo.Persistence.Exceptions;

namespace RepositoryPatternDemo.Persistence.Entities;

internal class Tag : IEntity, IComparable<Tag>
{
    public Guid? Id { get; set; }

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

    private string _description;
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            ValidateDescription();
        }
    }

    private Dictionary<string, List<string>> Errors { get; set; } = new();

    private Tag() { }

    public Tag(Guid? id, string slug, string name, string description)
    {
        Id = id;
        Slug = slug;
        Name = name;
        Description = description;

        foreach (var error in Errors)
        {
            if (error.Value.Count > 0)
                throw new EntityValidationException(Errors);
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

    private void ValidateName()
    {
        Errors[nameof(Name)] = new List<string>();

        if (string.IsNullOrWhiteSpace(Name))
        {
            Errors[nameof(Name)].Add("Name cannot be empty.");
        }
    }

    private void ValidateDescription()
    {
        Errors[nameof(Description)] = new List<string>();

        if (string.IsNullOrWhiteSpace(Description))
        {
            Errors[nameof(Description)].Add("Description cannot be empty.");
        }
    }

    public int CompareTo(Tag? other)
    {
        return Name.CompareTo(other?.Name);
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
        return $"Tag: {Name}, Slug: {Slug}, Description: {Description}, Id: {Id}";
    }
}
