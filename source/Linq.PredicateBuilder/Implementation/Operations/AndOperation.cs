namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <inheritdoc cref="IAndLogicOperation{TEntity}" />
    internal class AndOperation<TEntity> : LogicOperation<TEntity>, IAndLogicOperation<TEntity>
    {
        /// <inheritdoc/>
        public AndOperation(QueryBuilderResult<TEntity> builderResult, IOperationStrategy strategy)
            : base(builderResult, strategy)
        {
        }

        /// <inheritdoc/>
        public IAndLogicOperation<TEntity> Not => new NotAndOperation<TEntity>(Operation, Strategy);

        /// <inheritdoc />
        protected override Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> Operation =>
            x => BuilderResult.GetExpression().And(x);

        /// <inheritdoc/>
        public IAndQueryBuilderResult<TEntity> Equals<TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            TInput input)
            => EqualsInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IAndQueryBuilderResult<TEntity> Where(
            Expression<Func<TEntity, bool>> predicate)
            => WhereInternal(predicate);

        /// <inheritdoc/>
        public IAndQueryBuilderResult<TEntity> Where<TInput>(
            Expression<Func<TEntity, TInput, bool>> predicate,
            TInput input)
            => WhereInternal(predicate, input);

        /// <inheritdoc/>
        public IAndQueryBuilderResult<TEntity> In<TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            IEnumerable<TInput> input)
            => InInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IAndQueryBuilderResult<TEntity> Any<TInput>(
            Expression<Func<TEntity, ICollection<TInput>>> propertyExpression,
            Func<ILogicOperation<TInput>, IQueryBuilderResult<TInput>> builder)
            => AnyInternal(propertyExpression, builder);

        /// <inheritdoc/>
        public IAndQueryBuilderResult<TEntity> Brackets(
            Func<ILogicOperation<TEntity>, IQueryBuilderResult<TEntity>> builder)
            => BracketsInternal(builder);

        /// <inheritdoc/>
        public IAndQueryBuilderResult<TEntity> Contains(
            Expression<Func<TEntity, string>> propertyExpression,
            string input) => ContainsInternal(propertyExpression, input);

        /// <inheritdoc />
        public IAndLogicOperation<TEntity> Conditional(bool condition)
            => new ConditionAndOperation<TEntity>(Operation, condition, Strategy);
    }
}