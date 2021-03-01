namespace Linq.PredicateBuilder
{
    /// <inheritdoc cref="IQueryBuilderResult{TEntity}"/>
    public interface IAndOrQueryBuilderResult<TEntity> : IAndQueryBuilderResult<TEntity>, IOrQueryBuilderResult<TEntity>
    {
    }
}