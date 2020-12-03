namespace Linq.PredicateBuilder
{
    /// <summary>
    /// Defines methods of logical operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the data in the data source.</typeparam>
    public interface ILogicOperation<TEntity> : ILogicOperationT<IAndOrQueryBuilderResult<TEntity>, TEntity>
    {
        /// <summary>
        /// Logic NOT.
        /// </summary>
        ILogicOperation<TEntity> Not { get; }

        /// <summary>
        /// Добавляет логическую операцию только при выполнении услович
        /// </summary>
        /// <param name="condition">Условие добавления операции</param>
        ILogicOperation<TEntity> Conditional(bool condition);
    }
}