namespace Linq.PredicateBuilder
{
    /// <inheritdoc cref="IQueryBuilderResult{TEntity}"/>
    public interface IAndQueryBuilderResult<TEntity> : IQueryBuilderResult<TEntity>
    {
        /// <summary>
        /// Represents logical AND.
        /// </summary>
        IAndLogicOperation<TEntity> And { get; }
    }
}