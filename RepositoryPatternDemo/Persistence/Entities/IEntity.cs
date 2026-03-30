namespace RepositoryPatternDemo.Persistence.Entities;

internal interface IEntity
{
    Guid? Id { get; set; }
}