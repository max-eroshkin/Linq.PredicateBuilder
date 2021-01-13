namespace Linq.PredicateBuilder
{
    /// <inheritdoc cref="IQueryBuilderResult{TEntity}"/>
    public interface IOrQueryBuilderResult<TEntity> : IQueryBuilderResult<TEntity>
    {
        /// <summary>
        /// Represents logical OR.
        /// </summary>
        IOrLogicOperation<TEntity> Or { get; }
    }
}