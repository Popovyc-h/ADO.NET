namespace RepositoryPatternDemo.Persistence.Entities.Comparers;

internal class PostCompererByTitle : IComparer<Post>
{
    public int Compare(Post? first, Post? second)
    {
        return first?.Title.CompareTo(second?.Title) ?? 0;
    }
}
