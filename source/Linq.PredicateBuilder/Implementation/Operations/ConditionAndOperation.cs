namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Операция отрицания
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности в выражении</typeparam>
    internal class ConditionAndOperation<TEntity> : ConditionOperationBase<TEntity>, IAndLogicOperation<TEntity>
    {
        /// <inheritdoc />
        public ConditionAndOperation(
            Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> baseOperation,
            bool condition,
            IOperationStrategy strategy)
            : base(baseOperation, condition, strategy)
        {
        }

        /// <inheritdoc />
        public IAndLogicOperation<TEntity> Not => new NotAndOperation<TEntity>(Operation, Strategy);

        /// <inheritdoc/>
        public IAndQueryBuilderResult<TEntity> Equals<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            TValue input)
            => EqualsInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IAndQueryBuilderResult<TEntity> Where(
            Expression<Func<TEntity, bool>> predicate)
            => WhereInternal(predicate);

        /// <inheritdoc/>
        public IAndQueryBuilderResult<TEntity> In<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            IEnumerable<TValue> input)
            => InInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IAndQueryBuilderResult<TEntity> Any<TValue>(
            Expression<Func<TEntity, ICollection<TValue>>> manyToManySelector,
            Func<ILogicOperation<TValue>, IQueryBuilderResult<TValue>> builder)
            => AnyInternal(manyToManySelector, builder);

        /// <inheritdoc/>
        public IAndQueryBuilderResult<TEntity> Brackets(
            Func<ILogicOperation<TEntity>, IQueryBuilderResult<TEntity>> builder)
            => BracketsInternal(builder);

        /// <inheritdoc/>
        public IAndLogicOperation<TEntity> Conditional(bool condition)
            => new ConditionAndOperation<TEntity>(Operation, condition, Strategy);

        /// <inheritdoc />
        public IAndQueryBuilderResult<TEntity> Contains(
            Expression<Func<TEntity, string>> propertyExpression,
            string input)
            => ContainsInternal(propertyExpression, input);
    }
}