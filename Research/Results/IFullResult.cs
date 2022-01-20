namespace Linq.PredicateBuilder;

public interface IFullResult<TEntity> : IAndResult<TEntity>, IOrResult<TEntity>
{
}