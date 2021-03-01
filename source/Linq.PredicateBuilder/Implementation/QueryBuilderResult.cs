namespace Linq.PredicateBuilder
{
    using System;
    using System.Linq.Expressions;

    /// <inheritdoc cref="IQueryBuilderResult{TEntity}"/>
    internal class QueryBuilderResult<TEntity> : IAndOrQueryBuilderResult<TEntity>
    {
        private readonly Expression<Func<TEntity, bool>> _predicate;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="predicate">The filtering expression.</param>
        /// <param name="strategy">A filtering strategy.</param>
        public QueryBuilderResult(Expression<Func<TEntity, bool>> predicate, IOperationStrategy strategy)
        {
            _predicate = predicate;
            Strategy = strategy;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="strategy">A filtering strategy.</param>
        public QueryBuilderResult(IOperationStrategy strategy)
            : this(null, strategy)
        {
        }

        /// <inheritdoc />
        public IAndLogicOperation<TEntity> And => new AndOperation<TEntity>(this, Strategy);

        /// <inheritdoc />
        public IOrLogicOperation<TEntity> Or => new OrOperation<TEntity>(this, Strategy);

        private IOperationStrategy Strategy { get; }

        /// <inheritdoc />
        public Expression<Func<TEntity, bool>> GetExpression()
        {
            return _predicate;
        }
    }
}
