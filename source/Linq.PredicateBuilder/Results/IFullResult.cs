namespace Linq.PredicateBuilder;

/// <inheritdoc cref="IResult{TEntity}" />
public interface IFullResult<TEntity> : IAndResult<TEntity>, IOrResult<TEntity>
{
}