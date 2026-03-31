using RepositoryPatternDemo.Persistence.Entities;

namespace RepositoryPatternDemo.Persistence.Repositories.Contracts;

internal interface ITagRepository : IRepository<Tag>
{
    Tag? GetBySlug(string slug);
    //List<Post> GetPosts(Tag tag);
}
