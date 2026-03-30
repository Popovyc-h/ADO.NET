namespace RepositoryPatternDemo.Persistence.Entities.Comparators;

internal class UserCompererByCreatedAt : IComparer<User>
{
    public int Compare(User? first, User? second) => second?.CreatedAt.CompareTo(first?.CreatedAt) ?? 0;
}
