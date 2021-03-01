namespace Linq.PredicateBuilder
{
    /// <summary>
    /// Defines methods of logical operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the data in the data source.</typeparam>
    public interface IAndLogicOperation<TEntity> : ILogicOperationT<IAndQueryBuilderResult<TEntity>, TEntity>
    {
        /// <summary>
        /// Negates the first trailing builder expression.
        /// </summary>
        IAndLogicOperation<TEntity> Not { get; }

        /// <summary>
        /// Evaluates the first trailing builder expression if only the specified condition evaluates to true.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        IAndLogicOperation<TEntity> Conditional(bool condition);
    }
}