namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <inheritdoc cref="ILogicOperation{TEntity}" />
    internal class NotOperation<TEntity> : NotOperationBase<TEntity>, ILogicOperation<TEntity>
    {
        /// <inheritdoc />
        public NotOperation(
            Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> operation,
            IOperationStrategy strategy)
            : base(operation, strategy)
        {
        }

        /// <inheritdoc />
        public ILogicOperation<TEntity> Not => new NotOperation<TEntity>(Operation, Strategy);

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
        public IAndOrQueryBuilderResult<TEntity> Where<TValue>(
            Expression<Func<TEntity, TValue, bool>> predicate,
            TValue input)
            => WhereInternal(predicate, input);

        /// <inheritdoc/>
        public IAndOrQueryBuilderResult<TEntity> In<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            IEnumerable<TValue> input)
            => InInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IAndOrQueryBuilderResult<TEntity> Any<TValue>(
            Expression<Func<TEntity, ICollection<TValue>>> propertyExpression,
            Func<ILogicOperation<TValue>, IQueryBuilderResult<TValue>> builder)
            => AnyInternal(propertyExpression, builder);

        /// <inheritdoc/>
        public IAndOrQueryBuilderResult<TEntity> Brackets(
            Func<ILogicOperation<TEntity>, IQueryBuilderResult<TEntity>> builder)
            => BracketsInternal(builder);

        /// <inheritdoc/>
        public ILogicOperation<TEntity> Conditional(bool condition)
            => new ConditionOperation<TEntity>(Operation, condition, Strategy);

        /// <inheritdoc />
        public IAndOrQueryBuilderResult<TEntity> Contains(
            Expression<Func<TEntity, string>> propertyExpression,
            string input)
            => ContainsInternal(propertyExpression, input);
    }
}