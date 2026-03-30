using RepositoryPatternDemo.Persistence.Entities;
using RepositoryPatternDemo.Persistence.Repositories.Contracts;
using RepositoryPatternDemo.Persistence.Repositories.Generics;

namespace RepositoryPatternDemo.Persistence.Repositories.FileImpl;

internal class TagFileRepository : GenericFileRepository<Tag>, ITagRepository
{
    public TagFileRepository() : base(Path.Combine("Data", "tags.txt")) { }

    public Tag? GetBySlug(string slug)
    {
        return Find(t => t.Slug == slug);
    }

    public List<Post> GetPosts(Tag tag)
    {
        throw new NotImplementedException();
    }

    protected override Tag DeserializeEntity(string line)
    {
        string[] tagFields = line.Split("|");
        return new Tag(
                id: Guid.Parse(tagFields[0]),
                slug: tagFields[1],
                name: tagFields[2],
                description: tagFields[3]
            );
    }

    protected override string SerializeEntity(Tag tag)
    {
        return $"{tag.Id}|{tag.Slug}|{tag.Name}|{tag.Description}";
    }
}
