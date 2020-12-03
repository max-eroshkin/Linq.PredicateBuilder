namespace Linq.PredicateBuilder
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Промежуточный билдер выражения
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности в выражении</typeparam>
    public class QueryBuilderResult<TEntity> : IAndQueryBuilderResult<TEntity>, IOrQueryBuilderResult<TEntity>
    {
        private readonly Expression<Func<TEntity, bool>> _predicate;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="predicate">Условие, на основе которого строится итоговое выражение</param>
        /// <param name="strategy">A filtering strategy.</param>
        public QueryBuilderResult(Expression<Func<TEntity, bool>> predicate, IOperationStrategy strategy)
         : this(strategy)
        {
            _predicate = predicate;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="strategy">A filtering strategy.</param>
        public QueryBuilderResult(IOperationStrategy strategy)
        {
            _predicate = null;
            Strategy = strategy;
        }

        /// <summary>
        /// Операция логического "И"
        /// </summary>
        public IAndLogicOperation<TEntity> And => new AndOperation<TEntity>(this, Strategy);

        /// <summary>
        /// Операция логического "Или"
        /// </summary>
        public IOrLogicOperation<TEntity> Or => new OrOperation<TEntity>(this, Strategy);

        private IOperationStrategy Strategy { get; }

        /// <inheritdoc />
        public Expression<Func<TEntity, bool>> GetExpression()
        {
            return _predicate;
        }
    }
}
