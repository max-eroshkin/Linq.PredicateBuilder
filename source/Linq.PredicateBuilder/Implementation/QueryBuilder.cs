namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Билдер выражения поиска
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности в выражении</typeparam>
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
        public IAndOrQueryBuilderResult<TEntity> Equals<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            TValue input)
            => EqualsInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IAndOrQueryBuilderResult<TEntity> Where(
            Expression<Func<TEntity, bool>> predicate)
            => WhereInternal(predicate);

        /// <inheritdoc/>
        public IAndOrQueryBuilderResult<TEntity> In<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            IEnumerable<TValue> input)
            => InInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IAndOrQueryBuilderResult<TEntity> Any<TValue>(
            Expression<Func<TEntity, ICollection<TValue>>> manyToManySelector,
            Func<ILogicOperation<TValue>, IQueryBuilderResult<TValue>> builder)
            => AnyInternal(manyToManySelector, builder);

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