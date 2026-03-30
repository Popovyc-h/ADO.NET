using RepositoryPatternDemo.Persistence.Entities;

namespace RepositoryPatternDemo.Persistence.Repositories.Contracts;

internal interface IPostRepository : IRepository<Post>
{
    Post? GetBySlug(string slug);
    // Todo: by title, filters....
}
