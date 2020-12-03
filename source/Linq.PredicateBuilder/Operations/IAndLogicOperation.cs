namespace Linq.PredicateBuilder
{
    /// <summary>
    /// Defines methods of logical operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the data in the data source.</typeparam>
    public interface IAndLogicOperation<TEntity> : ILogicOperationT<IAndQueryBuilderResult<TEntity>, TEntity>
    {
        /// <summary>
        /// Logic NOT.
        /// </summary>
        IAndLogicOperation<TEntity> Not { get; }

        /// <summary>
        /// Добавляет логическую операцию только при выполнении услович
        /// </summary>
        /// <param name="condition">Условие добавления операции</param>
        IAndLogicOperation<TEntity> Conditional(bool condition);
    }
}