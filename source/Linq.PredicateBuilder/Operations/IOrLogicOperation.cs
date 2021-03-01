namespace Linq.PredicateBuilder
{
    /// <summary>
    /// Defines methods of logical operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the data in the data source.</typeparam>
    public interface IOrLogicOperation<TEntity> : ILogicOperationT<IOrQueryBuilderResult<TEntity>, TEntity>
    {
        /// <summary>
        /// Represents logical NOT.
        /// </summary>
        IOrLogicOperation<TEntity> Not { get; }

        /// <summary>
        /// Evaluates the first trailing builder expression if only the specified condition evaluates to true.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        IOrLogicOperation<TEntity> Conditional(bool condition);
    }
}