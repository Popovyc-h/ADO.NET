namespace RepositoryPatternDemo.Persistence.Exceptions;

internal class EntityValidationException : ArgumentException
{
    public readonly Dictionary<string, List<string>> errors;
    public EntityValidationException(Dictionary<string, List<string>> errors) => this.errors = errors;
}