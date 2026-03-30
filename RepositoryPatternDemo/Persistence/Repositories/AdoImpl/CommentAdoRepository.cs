using RepositoryPatternDemo.Persistence.Entities;
using RepositoryPatternDemo.Persistence.Repositories.Contracts;
using RepositoryPatternDemo.Persistence.Repositories.Generics;

namespace RepositoryPatternDemo.Persistence.Repositories.AdoImpl;

internal class CommentAdoRepository :
    GenericAdoRepository<Comment>,
    ICommentRepository
{
    public CommentAdoRepository(ConnectionManager connectionManager) :
        base(connectionManager, "Comments")
    { }
}