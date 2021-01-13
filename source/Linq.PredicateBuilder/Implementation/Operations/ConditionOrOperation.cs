namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <inheritdoc cref="ILogicOperation{TEntity}" />
    public class ConditionOrOperation<TEntity> : ConditionOperationBase<TEntity>, IOrLogicOperation<TEntity>
    {
        /// <inheritdoc />
        public ConditionOrOperation(
            Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> baseOperation,
            bool condition,
            IOperationStrategy strategy)
            : base(baseOperation, condition, strategy)
        {
        }

        /// <inheritdoc />
        public IOrLogicOperation<TEntity> Not => new NotOrOperation<TEntity>(Operation, Strategy);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> Equals<TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            TInput input)
            => EqualsInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> Where(
            Expression<Func<TEntity, bool>> predicate)
            => WhereInternal(predicate);

        /// <inheritdoc />
        public IOrQueryBuilderResult<TEntity> Where<TInput>(
            Expression<Func<TEntity, TInput, bool>> predicate,
            TInput input)
            => WhereInternal(predicate, input);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> In<TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            IEnumerable<TInput> input)
            => InInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> Any<TInput>(
            Expression<Func<TEntity, ICollection<TInput>>> propertyExpression,
            Func<ILogicOperation<TInput>, IQueryBuilderResult<TInput>> builder)
            => AnyInternal(propertyExpression, builder);

        /// <inheritdoc />
        public IOrQueryBuilderResult<TEntity> Brackets(
            Func<ILogicOperation<TEntity>, IQueryBuilderResult<TEntity>> builder)
            => BracketsInternal(builder);

        /// <inheritdoc/>
        public IOrLogicOperation<TEntity> Conditional(bool condition)
            => new ConditionOrOperation<TEntity>(Operation, condition, Strategy);

        /// <inheritdoc />
        public IOrQueryBuilderResult<TEntity> Contains(
            Expression<Func<TEntity, string>> propertyExpression,
            string input)
            => ContainsInternal(propertyExpression, input);
    }
}