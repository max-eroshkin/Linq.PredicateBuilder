namespace Research;

public interface IFullResult<TEntity> : IAndResult<TEntity>, IOrResult<TEntity>
{
}