namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Query builder.
    /// </summary>
    /// <typeparam name="TEntity">The type of the data in the data source.</typeparam>
    internal class QueryBuilder<TEntity> : LogicOperation<TEntity>, ILogicOperation<TEntity>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="strategy">A filtering strategy.</param>
        public QueryBuilder(IOperationStrategy strategy)
            : base(new QueryBuilderResult<TEntity>(strategy), strategy)
        {
        }

        /// <inheritdoc />
        public ILogicOperation<TEntity> Not => new NotOperation<TEntity>(Operation, Strategy);

        /// <inheritdoc />
        protected override Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> Operation => x => x;

        /// <inheritdoc/>
        public IAndOrQueryBuilderResult<TEntity> Equals<TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            TInput input)
            => EqualsInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IAndOrQueryBuilderResult<TEntity> Where(
            Expression<Func<TEntity, bool>> predicate)
            => WhereInternal(predicate);

        /// <inheritdoc/>
        public IAndOrQueryBuilderResult<TEntity> Where<TInput>(
            Expression<Func<TEntity, TInput, bool>> predicate,
            TInput input)
            => WhereInternal(predicate, input);

        /// <inheritdoc/>
        public IAndOrQueryBuilderResult<TEntity> In<TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            IEnumerable<TInput> input)
            => InInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IAndOrQueryBuilderResult<TEntity> Any<TInput>(
            Expression<Func<TEntity, ICollection<TInput>>> propertyExpression,
            Func<ILogicOperation<TInput>, IQueryBuilderResult<TInput>> builder)
            => AnyInternal(propertyExpression, builder);

        /// <inheritdoc />
        public IAndOrQueryBuilderResult<TEntity> Brackets(
            Func<ILogicOperation<TEntity>, IQueryBuilderResult<TEntity>> builder)
            => BracketsInternal(builder);

        /// <inheritdoc/>
        public IAndOrQueryBuilderResult<TEntity> Contains(
            Expression<Func<TEntity, string>> propertyExpression,
            string input) => ContainsInternal(propertyExpression, input);

        /// <inheritdoc />
        public ILogicOperation<TEntity> Conditional(bool condition)
            => new ConditionOperation<TEntity>(Operation, condition, Strategy);
    }
}