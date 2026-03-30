using RepositoryPatternDemo.Persistence.Entities;
using RepositoryPatternDemo.Persistence.Repositories.Contracts;
using RepositoryPatternDemo.Persistence.Repositories.Generics;
using RepositoryPatternDemo.Persistence.Repositories.Impl;

namespace RepositoryPatternDemo.Persistence.Repositories.FileImpl;

internal class CommentFileRepository : GenericFileRepository<Comment>, ICommentRepository
{
    private readonly UserFileRepository _userFileRepository;
    private readonly PostFileRepository _postFileRepository;

    public CommentFileRepository(
        UserFileRepository userFileRepository,
        PostFileRepository postFileRepository
        ) : base(Path.Combine("Data", "comments.txt"))
    {
        this._userFileRepository = userFileRepository;
        this._postFileRepository = postFileRepository;
    }

    protected override Comment DeserializeEntity(string line)
    {
        string[] commentFields = line.Split("|");
        return new Comment(
            id: Guid.Parse(commentFields[0]),
            user: _userFileRepository.Get(Guid.Parse(commentFields[1]))!,
            post: _postFileRepository.Get(Guid.Parse(commentFields[2]))!,
            body: commentFields[3],
            createdAt: DateTime.Parse(commentFields[4]),
            updatedAt: DateTime.Parse(commentFields[5])
            );
    }

    protected override string SerializeEntity(Comment comment)
    {
        return $"{comment.Id}|{comment.User.Id}|{comment.Post.Id}|{comment.Body}|{comment.CreatedAt}|{comment.UpdatedAt}";
    }
}
