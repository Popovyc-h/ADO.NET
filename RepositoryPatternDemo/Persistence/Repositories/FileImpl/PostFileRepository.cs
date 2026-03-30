using RepositoryPatternDemo.Persistence.Entities;
using RepositoryPatternDemo.Persistence.Repositories.Contracts;
using RepositoryPatternDemo.Persistence.Repositories.Generics;
using RepositoryPatternDemo.Persistence.Repositories.Impl;

namespace RepositoryPatternDemo.Persistence.Repositories.FileImpl;

internal class PostFileRepository : GenericFileRepository<Post>, IPostRepository
{
    private readonly UserFileRepository _userFileRepository;
    private readonly TagFileRepository _tagFileRepository;

    public PostFileRepository(
        UserFileRepository userFileRepository,
        TagFileRepository tagFileRepository) : base(Path.Combine("Data", "posts.txt"))
    {
        this._userFileRepository = userFileRepository;
        this._tagFileRepository = tagFileRepository;
    }

    public Post? GetBySlug(string slug)
    {
        return Find(t => t.Slug == slug);
    }

    protected override Post DeserializeEntity(string line)
    {
        string[] postFields = line.Split("|");
        Post newPost = new Post(
                id: Guid.Parse(postFields[0]),
                user: _userFileRepository.Get(Guid.Parse(postFields[1])),
                slug: postFields[2],
                title: postFields[3],
                body: postFields[4],
                image: !string.IsNullOrEmpty(postFields[5]) ? postFields[5] : null,
                publishedAt: DateTime.Parse(postFields[6]),
                createdAt: DateTime.Parse(postFields[7]),
                updatedAt: DateTime.Parse(postFields[8])
            );

        List<Tag?> tags = postFields[10].Split(",").Select(t => _tagFileRepository.Get(Guid.Parse(t))).ToList();

        newPost.Tags = tags;
        return newPost;
    }

    protected override string SerializeEntity(Post post)
    {
        string tagIds = string.Join(",", post.Tags.Select(tag => tag.Id));
        return $"{post.Id}|{post.User.Id}|{post.Slug}|{post.Title}|{post.Body}|{post.Image}|{post.PublishedAt}|{post.CreatedAt}|{post.UpdatedAt}|{tagIds}";
    }
}
