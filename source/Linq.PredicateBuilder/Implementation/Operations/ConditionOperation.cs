namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Операция отрицания
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности в выражении</typeparam>
    public class ConditionOperation<TEntity> : ConditionOperationBase<TEntity>, ILogicOperation<TEntity>
    {
        /// <inheritdoc />
        public ConditionOperation(
            Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> baseOperation,
            bool condition,
            IOperationStrategy strategy)
            : base(baseOperation, condition, strategy)
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
        public ILogicOperation<TEntity> Conditional(bool condition)
            => new ConditionOperation<TEntity>(Operation, condition, Strategy);

        /// <inheritdoc />
        public IAndOrQueryBuilderResult<TEntity> Contains(
            Expression<Func<TEntity, string>> propertyExpression,
            string input)
            => ContainsInternal(propertyExpression, input);
    }
}